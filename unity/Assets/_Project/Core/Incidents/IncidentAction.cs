using UnityEngine;

namespace Valkirie.Core.Incidents
{
    // Composable building block, same shape as PowerEffect/ObjectiveEffect: "spawn some
    // monsters" and "raise severity" are both just actions a trigger can fire.
    public abstract class IncidentAction : ScriptableObject
    {
        public abstract void Execute(IncidentContext context);
    }
}
