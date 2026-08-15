using UnityEngine;

namespace Valkirie.Core.Abilities
{
    // Free-plane movement with gravity disabled; some velocity inertia (MoveTowards, not
    // an instant snap) so it reads as flight rather than a hovering drone.
    [RequireComponent(typeof(Rigidbody2D))]
    public class AerialMotor2D : MonoBehaviour, IMotor, IMotorHandoffReceiver
    {
        [SerializeField] private AerialMotorConfig config;

        private Rigidbody2D _rb;
        private Vector2 _moveInput;
        private float _speedMultiplier = 1f;
        private bool _boosting;
        private float _knockbackTimer;

        public MotorCapabilities Capabilities => MotorCapabilities.Fly;
        public Vector3 Position => transform.position;
        public Vector3 Velocity => _rb.velocity;
        public Quaternion Rotation => transform.rotation;
        public bool IsGrounded => false;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0f;
        }

        private void FixedUpdate()
        {
            if (_knockbackTimer > 0f)
            {
                _knockbackTimer -= Time.fixedDeltaTime;
                return;
            }

            var speed = config.moveSpeed * _speedMultiplier * (_boosting ? config.boostMultiplier : 1f);
            var target = new Vector2(
                _moveInput.x * speed,
                _moveInput.y * speed * config.verticalSpeedMultiplier);
            var rate = target.sqrMagnitude > 0.01f ? config.acceleration : config.deceleration;
            _rb.velocity = Vector2.MoveTowards(_rb.velocity, target, rate * Time.fixedDeltaTime);
        }

        public void Move(Vector3 inputDirection) => _moveInput = inputDirection;

        public void SetBoosting(bool boosting) => _boosting = boosting;

        public void Jump() { } // flight has no discrete jump

        public void ApplyImpulse(Vector3 force) => _rb.AddForce(force, ForceMode2D.Impulse);

        public void SetSpeedMultiplier(float multiplier) => _speedMultiplier = multiplier;

        public void Knockback(Vector3 force, float duration)
        {
            _knockbackTimer = duration;
            _rb.velocity = force;
        }

        public void ReceiveHandoff(Vector3 position, Vector3 velocity)
        {
            transform.position = position;
            _rb.velocity = velocity;
        }
    }
}
