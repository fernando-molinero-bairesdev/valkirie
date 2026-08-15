using UnityEngine;

namespace Valkirie.Core.Incidents
{
    [CreateAssetMenu(menuName = "Valkirie/Incidents/Actions/Raise Severity")]
    public class RaiseSeverityAction : IncidentAction
    {
        public override void Execute(IncidentContext context) => context.Incident.RaiseSeverity();
    }
}
