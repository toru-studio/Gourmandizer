using Godot;
using System;

public partial class player : CharacterBody2D
{
    public float moveSpeedMax = 150.0f;
    public float moveAcceleration = 20.0f;
    public float jumpVelocity = 400.0f;
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();


    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocityY = Velocity;
        Vector2 velocityX = Velocity;
        
        // Gravity.
        if (!IsOnFloor())
        {
            velocityY.Y += gravity * (float)delta;
        }

        velocityX.X = 0;
        velocityX.Y = 0;

        if (IsOnFloor())
        {
            //Left Right Movement
            if (Input.IsKeyPressed(Key.A))
            {
                if (velocityX.X < moveSpeedMax)
                {
                    velocityX.X += -moveAcceleration;
                }
            }
            else if (Input.IsKeyPressed(Key.D))
            {
                if (velocityX.X > -moveSpeedMax)
                {
                    velocityX.X += moveAcceleration;
                }
            }

            // Jump.
            if ((Input.IsKeyPressed(Key.Space) || Input.IsKeyPressed(Key.W)) && IsOnFloor())
                velocityY.Y = -jumpVelocity;
        }

        // Handle Velocity
        Velocity = velocityY;
        Velocity += velocityX; // Change += to = if we want air movement
        MoveAndSlide();
    }
}