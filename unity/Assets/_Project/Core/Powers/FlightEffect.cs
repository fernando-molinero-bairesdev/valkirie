using UnityEngine;
using Valkirie.Core.Abilities;

namespace Valkirie.Core.Powers
{
    // Doesn't touch physics directly - just tells the MotorSwitcher to hand off between
    // grounded and aerial motors. Unsupported (no Fly capability) is a warned no-op, not
    // a crash and not a built fallback conversion nothing currently needs.
    [CreateAssetMenu(menuName = "Valkirie/Powers/Effects/Flight")]
    public class FlightEffect : PowerEffect
    {
        public override void Apply(PowerEffectContext context)
        {
            if (!context.Motor.Capabilities.HasFlag(MotorCapabilities.Fly))
            {
                Debug.LogWarning($"{context.Owner.name}: FlightEffect applied but motor has no Fly capability.", context.Owner);
                return;
            }

            if (context.Motor is MotorSwitcher switcher)
                switcher.SwitchTo<AerialMotor2D>();
        }

        public override void Remove(PowerEffectContext context)
        {
            if (context.Motor is MotorSwitcher switcher)
                switcher.SwitchTo<PlatformerMotor2D>();
        }
    }
}
