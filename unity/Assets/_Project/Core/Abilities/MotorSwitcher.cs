using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Valkirie.Core.Abilities
{
    // A character can own several IMotor implementations (grounded + flying) and switch
    // between them; everything else in the game only ever talks to IMotor, so nothing
    // needs to know a switch happened.
    public class MotorSwitcher : MonoBehaviour, IMotor
    {
        [SerializeField] private List<MonoBehaviour> motors = new();

        private List<IMotor> _resolvedMotors;
        private IMotor _activeMotor;

        public IMotor ActiveMotor => _activeMotor;
        public MotorCapabilities Capabilities { get; private set; }

        private void Awake()
        {
            _resolvedMotors = motors.OfType<IMotor>().ToList();
            Capabilities = _resolvedMotors.Aggregate(MotorCapabilities.None, (acc, m) => acc | m.Capabilities);
            _activeMotor = _resolvedMotors.FirstOrDefault();
        }

        public void SwitchTo<T>() where T : class, IMotor
        {
            var target = _resolvedMotors.OfType<T>().FirstOrDefault();
            if (target == null)
            {
                Debug.LogWarning($"{name}: no motor of type {typeof(T).Name} available to switch to.", this);
                return;
            }

            HandOff(_activeMotor, target);
            _activeMotor = target;
        }

        private static void HandOff(IMotor from, IMotor to)
        {
            if (from == null || to == null || from == to) return;
            if (to is IMotorHandoffReceiver receiver)
                receiver.ReceiveHandoff(from.Position, from.Velocity);
        }

        public Vector3 Position => _activeMotor.Position;
        public Vector3 Velocity => _activeMotor.Velocity;
        public Quaternion Rotation => _activeMotor.Rotation;
        public bool IsGrounded => _activeMotor.IsGrounded;

        public void Move(Vector3 inputDirection) => _activeMotor.Move(inputDirection);
        public void Jump() => _activeMotor.Jump();
        public void ApplyImpulse(Vector3 force) => _activeMotor.ApplyImpulse(force);
        public void SetSpeedMultiplier(float multiplier) => _activeMotor.SetSpeedMultiplier(multiplier);
        public void Knockback(Vector3 force, float duration) => _activeMotor.Knockback(force, duration);

#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (var m in motors)
                if (m != null && m is not IMotor)
                    Debug.LogWarning($"{name}: '{m.GetType().Name}' does not implement IMotor and will be ignored.", this);
        }
#endif
    }
}
