using UnityEngine;

namespace Valkirie.Core.Abilities
{
    [System.Flags]
    public enum MotorCapabilities
    {
        None = 0,
        Walk = 1 << 0,
        Jump = 1 << 1,
        Fly = 1 << 2,
        WallCling = 1 << 3,
        Deform = 1 << 4,
        Swim = 1 << 5,
    }

    // Vector3/Quaternion (not Vector2) throughout: 2D motors only ever populate x/y,
    // but this is what lets a future 3D game reuse the ability/objective system unchanged.
    public interface IMotor
    {
        Vector3 Position { get; }
        Vector3 Velocity { get; }
        Quaternion Rotation { get; }
        bool IsGrounded { get; }
        MotorCapabilities Capabilities { get; }

        void Move(Vector3 inputDirection);
        void Jump();
        void ApplyImpulse(Vector3 force);
        void SetSpeedMultiplier(float multiplier);
        void Knockback(Vector3 force, float duration);
    }

    public interface IMotorHandoffReceiver
    {
        void ReceiveHandoff(Vector3 position, Vector3 velocity);
    }
}
