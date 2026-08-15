using System.Collections.Generic;
using System.Linq;

namespace Valkirie.Core.Objectives
{
    public enum ObjectiveState { Pending, Active, Completed, Failed }

    // Definition (asset) + Instance (runtime state) - same split used throughout Core.
    public class ObjectiveInstance
    {
        public ObjectiveDefinition Definition { get; }
        public ObjectiveState State { get; private set; } = ObjectiveState.Pending;
        public float Progress { get; private set; }

        public event System.Action<ObjectiveInstance> Completed;
        public event System.Action<ObjectiveInstance> Failed;

        private readonly List<ObjectiveConditionTracker> _trackers;
        private float _elapsed;

        public ObjectiveInstance(ObjectiveDefinition definition)
        {
            Definition = definition;
            _trackers = definition.conditions.Select(c => c.CreateTracker()).ToList();
            foreach (var tracker in _trackers)
            {
                tracker.Completed += OnTrackerChanged;
                tracker.Failed += OnTrackerChanged;
                tracker.ProgressChanged += OnTrackerChanged;
            }
        }

        public void Activate()
        {
            if (State != ObjectiveState.Pending) return;
            State = ObjectiveState.Active;
            foreach (var tracker in _trackers) tracker.Start();
        }

        public void Tick(float deltaTime)
        {
            if (State != ObjectiveState.Active) return;

            _elapsed += deltaTime;
            foreach (var tracker in _trackers) tracker.Tick(deltaTime);

            if (Definition.timeLimit > 0f && _elapsed >= Definition.timeLimit)
                Fail();
        }

        private void OnTrackerChanged()
        {
            Progress = _trackers.Count == 0 ? 0f : _trackers.Average(t => t.Progress);

            var complete = Definition.completionRule == CompletionRule.All
                ? _trackers.All(t => t.IsComplete)
                : _trackers.Any(t => t.IsComplete);

            if (complete) Complete();
            else if (_trackers.Any(t => t.IsFailed)) Fail();
        }

        private void Complete()
        {
            if (State == ObjectiveState.Completed) return;
            State = ObjectiveState.Completed;
            foreach (var tracker in _trackers) tracker.Stop();
            foreach (var effect in Definition.onCompleteEffects) effect.Apply();
            Completed?.Invoke(this);
        }

        private void Fail()
        {
            if (State == ObjectiveState.Failed) return;
            State = ObjectiveState.Failed;
            foreach (var tracker in _trackers) tracker.Stop();
            foreach (var effect in Definition.onFailEffects) effect.Apply();
            Failed?.Invoke(this);
        }
    }
}
