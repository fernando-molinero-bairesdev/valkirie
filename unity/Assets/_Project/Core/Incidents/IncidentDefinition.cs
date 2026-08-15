using System.Collections.Generic;
using UnityEngine;
using Valkirie.Core.Objectives;

namespace Valkirie.Core.Incidents
{
    public enum IncidentSeverity { Minor, Major, Catastrophic }

    // ObjectiveSet + severity + a spawn/escalation trigger list. A Valkirie level uses a
    // bare ObjectiveSet; the superhero game's emergencies use this.
    [CreateAssetMenu(menuName = "Valkirie/Incidents/Incident Definition")]
    public class IncidentDefinition : ObjectiveSet
    {
        public IncidentSeverity startingSeverity = IncidentSeverity.Minor;
        public AnimationCurve difficultyScalingCurve = AnimationCurve.Linear(1, 0, 8, 4);
        public List<IncidentTrigger> triggers = new();

        public int ScaleCount(int baseCount, int playerCount) =>
            baseCount + Mathf.RoundToInt(difficultyScalingCurve.Evaluate(playerCount));
    }
}
