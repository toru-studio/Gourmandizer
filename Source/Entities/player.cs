using Godot;
using System;

public partial class player : CharacterBody2D
{ 
	private Dragon Dragon;
	private bool Entered;
	
	public float MoveSpeed = 80.0f;
	public float MoveAcceleration = 10.0f;
	public float JumpVelocity = 600.0f;
	public float Friction = 10f;
	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public const float DefaultWeight = 25.0f;
	public float CurrentWeight = 15.0f;
	public int FoodItems = 0;

	public override void _Ready()
	{
		this.Dragon = (Dragon)this.GetParent().FindChild("Dragon");
		this.Entered = false;
		GD.Print("Player Dragon Instance ID: ", this.Dragon.GetInstanceId());
		GD.Print("Player Dragon Hash Code: ", this.Dragon.GetHashCode());
	}

	public override void _PhysicsProcess(double delta)
	{
		var velocity = Velocity;

		var moveSpeedMax = MoveSpeed - (CurrentWeight * 2); // Change with CurrentWeight

		// Handle jump
		if (Input.IsKeyPressed(Key.Space) && IsOnFloor())
		{
			velocity.Y = -JumpVelocity + CurrentWeight * 10;
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
		velocity.X = Mathf.Clamp(velocity.X, -moveSpeedMax, moveSpeedMax);
		Velocity = velocity;

		MoveAndSlide();
	}

	public override void _Process(double delta)
	{
		if (this.Entered)
		{
			if (this.FoodItems == 0 || !this.Dragon.Extended) {return;}
			Console.WriteLine("Weight - " + FoodItems);
			CurrentWeight -= FoodItems;
			this.Dragon.Expand(1.1f*FoodItems);
			FoodItems = 0;
		}
	}

	private void _on_offering_area_body_entered(Node2D body)
	{
		if (body.Name.Equals("PlayerCharacter"))
		{
			this.Entered = true;
		}
	}

	private void _on_offering_area_body_exited(Node2D body)
	{
		if (body.Name.Equals("PlayerCharacter"))
		{
			this.Entered = false;
		}
	}

	private void _on_food_body_entered(Node2D body)
	{		
		if (body.Name != "PlayerCharacter") return;
		CurrentWeight += 1;
		FoodItems += 1;
		Console.WriteLine("Weight is Now " + FoodItems);
	}
	
}
