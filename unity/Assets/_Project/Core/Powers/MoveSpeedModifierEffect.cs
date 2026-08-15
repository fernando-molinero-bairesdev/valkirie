using UnityEngine;

namespace Valkirie.Core.Powers
{
    [CreateAssetMenu(menuName = "Valkirie/Powers/Effects/Move Speed Modifier")]
    public class MoveSpeedModifierEffect : PowerEffect
    {
        public float multiplier = 1.5f;

        public override void Apply(PowerEffectContext context) =>
            context.Motor.SetSpeedMultiplier(multiplier);

        public override void Remove(PowerEffectContext context) =>
            context.Motor.SetSpeedMultiplier(1f);
    }
}
