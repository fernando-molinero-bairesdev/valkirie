using UnityEngine;
using Valkirie.Core.Events;

namespace Valkirie.Core.Objectives
{
    [CreateAssetMenu(menuName = "Valkirie/Objectives/Conditions/Reach Zone")]
    public class ReachZoneCondition : ObjectiveCondition
    {
        public StringGameEvent zoneEnteredEvent;
        public string zoneId;

        public override ObjectiveConditionTracker CreateTracker() =>
            new Tracker(zoneEnteredEvent, zoneId);

        private class Tracker : ObjectiveConditionTracker
        {
            private readonly StringGameEvent _event;
            private readonly string _zoneId;

            public Tracker(StringGameEvent gameEvent, string zoneId)
            {
                _event = gameEvent;
                _zoneId = zoneId;
            }

            public override void Start() => _event.Register(OnZoneEntered);
            public override void Stop() => _event.Unregister(OnZoneEntered);

            private void OnZoneEntered(string enteredZoneId)
            {
                if (enteredZoneId != _zoneId) return;
                ReportProgress(1f);
                MarkComplete();
            }
        }
    }
}
