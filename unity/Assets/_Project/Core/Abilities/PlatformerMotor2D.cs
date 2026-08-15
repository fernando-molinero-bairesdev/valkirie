using UnityEngine;

namespace Valkirie.Core.Abilities
{
    // Velocity-set movement (not force-accumulation) for tight, responsive platforming,
    // plus coyote time and jump buffering so input feels forgiving instead of floaty.
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlatformerMotor2D : MonoBehaviour, IMotor, IMotorHandoffReceiver
    {
        [SerializeField] private PlatformerMotorConfig config;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.1f;

        private Rigidbody2D _rb;
        private float _speedMultiplier = 1f;
        private float _moveInput;
        private float _lastGroundedTime = -999f;
        private float _jumpBufferedTime = -999f;
        private float _knockbackTimer;

        public MotorCapabilities Capabilities => MotorCapabilities.Walk | MotorCapabilities.Jump;
        public Vector3 Position => transform.position;
        public Vector3 Velocity => _rb.velocity;
        public Quaternion Rotation => transform.rotation;
        public bool IsGrounded { get; private set; }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = config.gravityScale;
        }

        private void Update()
        {
            IsGrounded = groundCheck != null && Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
            if (IsGrounded) _lastGroundedTime = Time.time;

            var withinBufferWindow = Time.time - _jumpBufferedTime <= config.jumpBufferTime;
            var withinCoyoteWindow = Time.time - _lastGroundedTime <= config.coyoteTime;
            if (withinBufferWindow && withinCoyoteWindow)
            {
                PerformJump();
                _jumpBufferedTime = -999f;
            }
        }

        private void FixedUpdate()
        {
            if (_knockbackTimer > 0f)
            {
                _knockbackTimer -= Time.fixedDeltaTime;
                return;
            }

            var targetSpeed = _moveInput * config.moveSpeed * _speedMultiplier;
            var rate = Mathf.Abs(targetSpeed) > 0.01f ? config.acceleration : config.deceleration;
            var newX = Mathf.MoveTowards(_rb.velocity.x, targetSpeed, rate * Time.fixedDeltaTime);
            _rb.velocity = new Vector2(newX, _rb.velocity.y);
        }

        public void Move(Vector3 inputDirection) => _moveInput = Mathf.Clamp(inputDirection.x, -1f, 1f);

        public void Jump() => _jumpBufferedTime = Time.time;

        private void PerformJump()
        {
            _rb.velocity = new Vector2(_rb.velocity.x, config.jumpForce);
            _lastGroundedTime = -999f;
        }

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
