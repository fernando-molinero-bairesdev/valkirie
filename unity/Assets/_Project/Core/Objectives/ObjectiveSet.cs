using System.Collections.Generic;
using UnityEngine;

namespace Valkirie.Core.Objectives
{
    // The bare minimum a level needs - e.g. a Valkirie battlefield's "collect 12 souls."
    // IncidentDefinition (Core/Incidents) extends this with severity/spawn table/escalation
    // for the superhero game's concurrent, reactive emergencies.
    [CreateAssetMenu(menuName = "Valkirie/Objectives/Objective Set")]
    public class ObjectiveSet : ScriptableObject
    {
        public List<ObjectiveDefinition> objectives = new();
        public CompletionRule completionRule = CompletionRule.All;
    }
}
