using UnityEngine;
using Valkirie.Core.Events;

namespace Valkirie.Core.Objectives
{
    // Reacts to a tagged event rather than polling - doesn't know or care whether the
    // thing being collected is a Valkirie soul or anything else.
    [CreateAssetMenu(menuName = "Valkirie/Objectives/Conditions/Collect Count")]
    public class CollectCountCondition : ObjectiveCondition
    {
        public StringGameEvent itemCollectedEvent;
        public string itemTag;
        public int requiredCount = 1;

        public override ObjectiveConditionTracker CreateTracker() =>
            new Tracker(itemCollectedEvent, itemTag, requiredCount);

        private class Tracker : ObjectiveConditionTracker
        {
            private readonly StringGameEvent _event;
            private readonly string _tag;
            private readonly int _required;
            private int _current;

            public Tracker(StringGameEvent gameEvent, string tag, int required)
            {
                _event = gameEvent;
                _tag = tag;
                _required = required;
            }

            public override void Start() => _event.Register(OnCollected);
            public override void Stop() => _event.Unregister(OnCollected);

            private void OnCollected(string collectedTag)
            {
                if (collectedTag != _tag) return;
                _current++;
                ReportProgress((float)_current / _required);
                if (_current >= _required) MarkComplete();
            }
        }
    }
}
