using UnityEngine;

namespace Valkirie.Core.Abilities
{
    [CreateAssetMenu(menuName = "Valkirie/Motors/Platformer Motor Config")]
    public class PlatformerMotorConfig : ScriptableObject
    {
        public float moveSpeed = 6f;
        public float acceleration = 60f;
        public float deceleration = 80f;
        public float jumpForce = 12f;
        public float gravityScale = 3f;
        public float coyoteTime = 0.1f;
        public float jumpBufferTime = 0.1f;
    }
}
