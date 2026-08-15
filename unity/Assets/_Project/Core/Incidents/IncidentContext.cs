namespace Valkirie.Core.Incidents
{
    public class IncidentContext
    {
        public readonly IncidentInstance Incident;
        public readonly int PlayerCount;

        public IncidentContext(IncidentInstance incident, int playerCount)
        {
            Incident = incident;
            PlayerCount = playerCount;
        }
    }
}
