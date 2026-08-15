using UnityEngine;
using Valkirie.Core.Powers;

namespace Valkirie.Core.Entities
{
    // Replaces the old pygame prototype's archetypes.json - a named, spawnable thing
    // (prefab + optional character composition) that levels and spawn tables reference.
    [CreateAssetMenu(menuName = "Valkirie/Entities/Entity Archetype")]
    public class EntityArchetype : ScriptableObject
    {
        public string id;
        public GameObject prefab;
        public CharacterDefinition characterDefinition;
    }
}
