using System.Collections.Generic;

namespace Valkirie.Core.Incidents
{
    // Runtime lookup that live SpawnZones register into, keyed by zoneId - this is what
    // lets an IncidentDefinition's actions reference zones by string id rather than a
    // direct scene reference, at the cost of a possible id mismatch (see ZoneIdRegistry).
    public static class IncidentSpawnZoneRegistry
    {
        private static readonly Dictionary<string, SpawnZone> Zones = new();

        public static void Register(SpawnZone zone)
        {
            if (string.IsNullOrEmpty(zone.zoneId)) return;
            Zones[zone.zoneId] = zone;
        }

        public static void Unregister(SpawnZone zone)
        {
            if (Zones.TryGetValue(zone.zoneId, out var registered) && registered == zone)
                Zones.Remove(zone.zoneId);
        }

        public static SpawnZone Find(string zoneId) =>
            Zones.TryGetValue(zoneId, out var zone) ? zone : null;
    }
}
