using Godot;
using System;

public partial class player : CharacterBody2D
{
    private float _moveSpeed = 50.0f;
    private float _moveAcceleration = 10.0f;
    private float _jumpVelocity = 400.0f;
    private float _friction = 10f;
    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    private const float DefaultWeight = 25.0f;
    private float _currentWeight = 15.0f;
    private int _foodItems = 10;
    private PackedScene _foodScene = GD.Load<PackedScene>("res://Source/Item/Food.tscn");
    

    public override void _Ready()
    {
        var foodInstance = _foodScene.Instantiate();

    }


    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;

        var moveSpeedMax = _moveSpeed - (_currentWeight * 2); // Change with CurrentWeight

        // Handle jump
        if (Input.IsKeyPressed(Key.Space) && IsOnFloor())
        {
            velocity.Y = -_jumpVelocity + _currentWeight * 10;
        }

        // Apply gravity
        if (!IsOnFloor())
        {
            velocity.Y += (_gravity + DefaultWeight) * (float)delta;
        }
        // Horizontal movement
        else if (Input.IsKeyPressed(Key.Left))
        {
            velocity.X -= _moveAcceleration;
        }
        else if (Input.IsKeyPressed(Key.Right))
        {
            velocity.X += _moveAcceleration;
        }
        else
        {
            //Friction
            velocity.X = Mathf.Lerp(velocity.X, 0, (_friction) * (float)delta);
        }

        // Limit horizontal speed
        velocity.X = Mathf.Clamp(velocity.X, -moveSpeedMax, moveSpeedMax);
        Velocity = velocity;

        MoveAndSlide();
    }

    private void _on_offering_area_body_entered(Node2D body)
    {
        if (body.Name != "PlayerCharacter") return;
        Console.WriteLine("Weight - " + _foodItems);
        _currentWeight -= _foodItems;
        _foodItems = 0;
    }

    private void _on_food_body_entered(Node2D body)
    {
        if (body.Name != "PlayerCharacter") return;
        _currentWeight += 1;
        _foodItems += 1;
        Console.WriteLine("Weight is Now " + _foodItems);
    }
}