using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valkirie.Core.Objectives;

namespace Valkirie.Core.Incidents
{
    public enum IncidentState { InProgress, Resolved, Failed }

    // Server-authoritative under Netcode for GameObjects: this class and its trigger
    // evaluation should only ever run on the server. Clients get a thin replicated
    // summary (objective id / state / progress) - see the per-game networking layer,
    // not yet wired up here.
    public class IncidentInstance
    {
        public IncidentDefinition Definition { get; }
        public IncidentState State { get; private set; } = IncidentState.InProgress;
        public IncidentSeverity Severity { get; private set; }

        public readonly List<ObjectiveInstance> Objectives = new();

        private readonly System.Func<int> _playerCountProvider;
        private float _elapsed;

        public IncidentInstance(IncidentDefinition definition, System.Func<int> playerCountProvider)
        {
            Definition = definition;
            Severity = definition.startingSeverity;
            _playerCountProvider = playerCountProvider;

            foreach (var objDef in definition.objectives)
            {
                var instance = new ObjectiveInstance(objDef);
                instance.Completed += _ => RunTriggers(IncidentConditionType.OnObjectiveActivated, objDef.id);
                instance.Failed += _ => RunTriggers(IncidentConditionType.OnObjectiveFailed, objDef.id);
                Objectives.Add(instance);
            }
        }

        public void Start()
        {
            foreach (var objective in Objectives) objective.Activate();
            RunTriggers(IncidentConditionType.OnIncidentStart, null);
        }

        public void Tick(float deltaTime)
        {
            if (State != IncidentState.InProgress) return;

            _elapsed += deltaTime;
            foreach (var objective in Objectives) objective.Tick(deltaTime);

            foreach (var trigger in Definition.triggers)
            {
                if (trigger.condition != IncidentConditionType.OnElapsedTime) continue;
                if (_elapsed < trigger.elapsedTimeSeconds) continue;
                if (!trigger.repeatable && trigger.lastFiredTime > -999f) continue;
                if (Time.time - trigger.lastFiredTime < trigger.cooldown) continue;

                Fire(trigger);
            }

            EvaluateOutcome();
        }

        private void RunTriggers(IncidentConditionType type, string objectiveId)
        {
            foreach (var trigger in Definition.triggers)
            {
                if (trigger.condition != type) continue;
                if (objectiveId != null && trigger.objectiveId != objectiveId) continue;
                if (!trigger.repeatable && trigger.lastFiredTime > -999f) continue;

                Fire(trigger);
            }
        }

        private void Fire(IncidentTrigger trigger)
        {
            trigger.lastFiredTime = Time.time;
            var context = new IncidentContext(this, _playerCountProvider?.Invoke() ?? 1);
            foreach (var action in trigger.actions)
                action.Execute(context);
        }

        public void RaiseSeverity()
        {
            if (Severity < IncidentSeverity.Catastrophic)
                Severity++;
        }

        private void EvaluateOutcome()
        {
            var required = Objectives.Where(o => !o.Definition.isOptional).ToList();
            if (required.Count == 0) return;

            if (required.All(o => o.State == ObjectiveState.Completed))
                State = IncidentState.Resolved;
            else if (required.Any(o => o.State == ObjectiveState.Failed))
                State = IncidentState.Failed;
        }
    }
}
