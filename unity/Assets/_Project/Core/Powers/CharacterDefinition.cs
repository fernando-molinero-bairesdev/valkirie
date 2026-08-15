using System.Collections.Generic;
using UnityEngine;

namespace Valkirie.Core.Powers
{
    // A hero (or enemy) is built by referencing existing PowerDefinition assets - a new
    // character is a new asset, not new code.
    [CreateAssetMenu(menuName = "Valkirie/Characters/Character Definition")]
    public class CharacterDefinition : ScriptableObject
    {
        public string displayName;
        public GameObject visualPrefab;
        public List<PowerDefinition> loadout = new();
    }
}
