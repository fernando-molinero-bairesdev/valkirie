using UnityEngine;
using Valkirie.Core.Abilities;

namespace Valkirie.Core.Powers
{
    // Small, composable, reusable building block. A PowerDefinition assembles a list of
    // these instead of a whole new script existing per power.
    public abstract class PowerEffect : ScriptableObject
    {
        public abstract void Apply(PowerEffectContext context);
        public abstract void Remove(PowerEffectContext context);
    }

    public readonly struct PowerEffectContext
    {
        public readonly IMotor Motor;
        public readonly GameObject Owner;

        public PowerEffectContext(IMotor motor, GameObject owner)
        {
            Motor = motor;
            Owner = owner;
        }
    }
}
