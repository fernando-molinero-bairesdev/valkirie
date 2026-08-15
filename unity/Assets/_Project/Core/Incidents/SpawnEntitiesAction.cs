using UnityEngine;
using Valkirie.Core.Entities;

namespace Valkirie.Core.Incidents
{
    [CreateAssetMenu(menuName = "Valkirie/Incidents/Actions/Spawn Entities")]
    public class SpawnEntitiesAction : IncidentAction
    {
        public EntityArchetype archetype;
        [ZoneId] public string zoneId;
        public int baseCount = 1;
        public bool scalesWithPlayerCount = true;

        public override void Execute(IncidentContext context)
        {
            var zone = IncidentSpawnZoneRegistry.Find(zoneId);
            if (zone == null)
            {
                Debug.LogWarning($"SpawnEntitiesAction: no SpawnZone found with id '{zoneId}'.");
                return;
            }

            var count = scalesWithPlayerCount
                ? context.Incident.Definition.ScaleCount(baseCount, context.PlayerCount)
                : baseCount;

            for (var i = 0; i < count; i++)
            {
                // TODO: pull from a pooled EntityPool instead of Instantiate/Destroy once
                // one exists - required before this scales to repeatable ambient waves.
                Object.Instantiate(archetype.prefab, zone.GetRandomPoint(), Quaternion.identity);
            }
        }
    }
}
