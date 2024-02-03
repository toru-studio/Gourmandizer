using Godot;
using System;

public partial class player : CharacterBody2D
{
	public float MoveSpeedMax = 5.0f;
	public float MoveAcceleration = 1.0f;
	public float JumpVelocity = 300.0f;
	public float Friction = 10f;
	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public const float DefaultWeight = 25.0f;
	public float CurrentWeight;

	public override void _PhysicsProcess(double delta)
	{
		var velocity = Velocity;
		
		MoveSpeedMax = 500 - (DefaultWeight * 2); // Change with CurrentWeight


		// Handle jump
		if (Input.IsKeyPressed(Key.Space) && IsOnFloor())
		{
			velocity.Y = -JumpVelocity;
		}
		// Apply gravity
		if (!IsOnFloor())
		{
			velocity.Y += (Gravity + DefaultWeight) * (float)delta;
		}
		// Horizontal movement
		else if (Input.IsKeyPressed(Key.Left))
		{
			velocity.X -= MoveAcceleration;
		}
		else if (Input.IsKeyPressed(Key.Right))
		{
			velocity.X += MoveAcceleration;
		}
		else
		{
			//Friction
			velocity.X = Mathf.Lerp(velocity.X, 0, (Friction) * (float)delta);

		}

		// Limit horizontal speed
		velocity.X = Mathf.Clamp(velocity.X, -MoveSpeedMax, MoveSpeedMax);
		
		
		Velocity = velocity;
		MoveAndSlide();
	}
}
