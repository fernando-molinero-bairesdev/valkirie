using System.Collections.Generic;

namespace Valkirie.Core.Incidents
{
    public enum IncidentConditionType
    {
        OnIncidentStart,
        OnElapsedTime,
        OnObjectiveActivated,
        OnObjectiveFailed,
        OnPlayerCountInZone,
        OnSeverityReached
    }

    // (condition) -> (actions). Spawn tables and escalation are the same mechanism: a
    // "spawn 3 monsters at start" trigger and an "if not resolved in 60s, spawn 2 more and
    // add an evacuation objective" trigger differ only in which condition/actions are set.
    [System.Serializable]
    public class IncidentTrigger
    {
        public IncidentConditionType condition;
        public float elapsedTimeSeconds;
        public string objectiveId;
        [ZoneId] public string zoneId;
        public int playerCountThreshold;
        public IncidentSeverity severityThreshold;
        public bool repeatable;
        public float cooldown;
        public List<IncidentAction> actions = new();

        [System.NonSerialized] public float lastFiredTime = -999f;
    }
}
