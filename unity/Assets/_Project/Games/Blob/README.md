# Blob (game)

Not yet scaffolded. Needs its own `SoftBodyMotor : IMotor` (point-ring of Rigidbody2D +
joints driving a procedural mesh) plus an `ISoftBodyMotor : IMotor` extension for the
deformation-specific verbs (Squish/Split/Merge) that only this game's gameplay code should
reference directly - see the motor abstraction discussion for why those don't belong on
the shared IMotor interface.
