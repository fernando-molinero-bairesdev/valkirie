using UnityEngine;

namespace Valkirie.Core.Incidents
{
    public class SpawnZone : MonoBehaviour
    {
        [ZoneId] public string zoneId;
        public Vector2 size = Vector2.one;

        private void OnEnable() => IncidentSpawnZoneRegistry.Register(this);
        private void OnDisable() => IncidentSpawnZoneRegistry.Unregister(this);

        public Vector3 GetRandomPoint()
        {
            var offset = new Vector3(
                Random.Range(-size.x / 2f, size.x / 2f),
                Random.Range(-size.y / 2f, size.y / 2f),
                0f);
            return transform.position + offset;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(zoneId))
                Debug.LogWarning($"{name}: SpawnZone has no zoneId set.", this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0f, 1f, 1f, 0.25f);
            Gizmos.DrawCube(transform.position, size);
        }
#endif
    }
}
