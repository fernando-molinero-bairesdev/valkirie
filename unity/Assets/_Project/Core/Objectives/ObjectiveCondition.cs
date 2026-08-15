using UnityEngine;

namespace Valkirie.Core.Objectives
{
    // Small, composable building block - same shape as PowerEffect. Add new condition
    // types as new games need them; Core never needs to change.
    public abstract class ObjectiveCondition : ScriptableObject
    {
        public abstract ObjectiveConditionTracker CreateTracker();
    }

    // Definitions are immutable shared assets; the tracker is the runtime, per-attempt
    // mutable counterpart - same split as PowerDefinition/PowerInstance.
    public abstract class ObjectiveConditionTracker
    {
        public float Progress { get; protected set; }
        public bool IsComplete { get; protected set; }
        public bool IsFailed { get; protected set; }

        public event System.Action Completed;
        public event System.Action Failed;
        public event System.Action ProgressChanged;

        public abstract void Start();
        public abstract void Stop();
        public virtual void Tick(float deltaTime) { }

        protected void MarkComplete()
        {
            if (IsComplete) return;
            IsComplete = true;
            Progress = 1f;
            Completed?.Invoke();
        }

        protected void MarkFailed()
        {
            if (IsFailed) return;
            IsFailed = true;
            Failed?.Invoke();
        }

        protected void ReportProgress(float value)
        {
            Progress = Mathf.Clamp01(value);
            ProgressChanged?.Invoke();
        }
    }
}
