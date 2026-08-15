using UnityEngine;

namespace Valkirie.Core.Abilities
{
    [CreateAssetMenu(menuName = "Valkirie/Motors/Aerial Motor Config")]
    public class AerialMotorConfig : ScriptableObject
    {
        public float moveSpeed = 8f;
        public float acceleration = 40f;
        public float deceleration = 50f;
        public float boostMultiplier = 1.8f;
        public float verticalSpeedMultiplier = 0.75f;
    }
}
