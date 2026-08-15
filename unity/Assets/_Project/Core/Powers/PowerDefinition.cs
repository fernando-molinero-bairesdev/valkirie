using System.Collections.Generic;
using UnityEngine;

namespace Valkirie.Core.Powers
{
    public enum PowerCategory { Offense, Mobility, Utility, Passive }

    // A designer assembles a new power from existing PowerEffect assets in the Inspector;
    // new code is only needed when a genuinely new mechanic doesn't exist yet.
    [CreateAssetMenu(menuName = "Valkirie/Powers/Power Definition")]
    public class PowerDefinition : ScriptableObject
    {
        public string displayName;
        public Sprite icon;
        public PowerCategory category;
        public float cooldown;
        public List<string> tags = new();
        public List<PowerEffect> effects = new();
    }
}
