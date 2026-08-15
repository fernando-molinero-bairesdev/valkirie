using UnityEngine;

namespace Valkirie.Core.Powers
{
    // PowerDefinition is an immutable shared asset; PowerInstance is the runtime,
    // per-character mutable counterpart (cooldown, active state). Never mutate the
    // ScriptableObject asset itself at runtime - it's shared across every instance.
    public class PowerInstance
    {
        public PowerDefinition Definition { get; }
        public bool IsActive { get; private set; }
        public float CooldownRemaining { get; private set; }

        public PowerInstance(PowerDefinition definition)
        {
            Definition = definition;
        }

        public bool TryActivate(PowerEffectContext context)
        {
            if (IsActive || CooldownRemaining > 0f) return false;

            IsActive = true;
            foreach (var effect in Definition.effects)
                effect.Apply(context);
            return true;
        }

        public void Deactivate(PowerEffectContext context)
        {
            if (!IsActive) return;

            foreach (var effect in Definition.effects)
                effect.Remove(context);
            IsActive = false;
            CooldownRemaining = Definition.cooldown;
        }

        public void Tick(float deltaTime)
        {
            if (CooldownRemaining > 0f)
                CooldownRemaining = Mathf.Max(0f, CooldownRemaining - deltaTime);
        }
    }
}
