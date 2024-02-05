using Godot;
using System;

public partial class player : CharacterBody2D
{
	public float MoveSpeed = 150.0f;
	public float MoveAcceleration = 30.0f;
	public float JumpVelocity = 400.0f;
	public float Friction = 10f;
	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public float CurrentWeight = 20.0f;
	public float FoodItems = 0.0f;

	private Dragon Dragon;
	private bool Entered;
	private AnimationPlayer AnimationPlayer;
	private Sprite2D Character;
	
	private AudioStreamPlayer2D CaveSteps;
	private AudioStreamPlayer2D Steps;
	private AudioStreamPlayer2D FlowerSound;

public override void _Ready()
	{
		this.Dragon = (Dragon)this.GetParent().FindChild("Dragon");
		this.Entered = false;
		this.AnimationPlayer = this.GetNode<AnimationPlayer>("AnimationPlayer");
		this.Character = this.GetNode<Sprite2D>("Character");
		this.CaveSteps = this.GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		this.Steps = this.GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D2");
		this.FlowerSound = this.GetNode<AudioStreamPlayer2D>("FlowerPickup");
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
			velocity.Y = -JumpVelocity + CurrentWeight * 3;
		}

		// Apply gravity
		if (!IsOnFloor())
		{
			velocity.Y += (Gravity + CurrentWeight*2) * (float)delta;
		}
		// Horizontal movement
		 if (Input.IsKeyPressed(Key.Left) || Input.IsKeyPressed(Key.A))
		{
			this.playFootSteps();
			this.Character.FlipH = true;
			this.Character.Position = new Vector2(-15, 2);
			AnimationPlayer.Play("walk_cycle");
			velocity.X -= MoveAcceleration;
		}
		else if (Input.IsKeyPressed(Key.Right) || Input.IsKeyPressed(Key.D))
		{
			this.playFootSteps();
			this.Character.FlipH = false;
			this.Character.Position = new Vector2(-10, 2);
			AnimationPlayer.Play("walk_cycle");
			velocity.X += MoveAcceleration;
		}
		else
		{
			this.stopFootSteps();
			this.Character.FlipH = false;
			this.Character.Position = new Vector2(-10, 2);
			AnimationPlayer.Play("idle");
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
			CurrentWeight -= FoodItems * 3f;
			this.Dragon.Expand(1.1f*FoodItems);
			FoodItems = 0;
		}
	}

	private void _on_offering_area_body_entered(Node2D body)
	{
		if (body.Name.Equals("PlayerCharacter"))
		{
			this.Entered = true;
			//CurrentWeight -= FoodItems * 2.5;
			//FoodItems = 0;
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
		this.FlowerSound.Play();
		CurrentWeight += 3f;
		FoodItems += 1;
		Console.WriteLine("Weight is Now " + CurrentWeight);
	}

	private void playFootSteps()
	{
		if (!IsOnFloor())
		{
			if (this.CaveSteps.Playing)
			{
				this.CaveSteps.Stop();
			}

			if (this.Steps.Playing)
			{
				this.Steps.Stop();
			}

			return;
		}
		if (this.GlobalPosition.Y < 680)
		{
			if (this.CaveSteps.Playing)
			{
				this.CaveSteps.Stop();
			}

			if (!this.Steps.Playing)
			{
				this.Steps.Play();
			}
		}
		else
		{
			if (this.Steps.Playing)
			{
				this.Steps.Stop();
			}

			if (!this.CaveSteps.Playing)
			{
				this.CaveSteps.Play();
			}
		}
	}

	private void stopFootSteps()
	{
		if (this.CaveSteps.Playing)
		{
			this.CaveSteps.Stop();
		}

		if (this.Steps.Playing)
		{
			this.Steps.Stop();
		}
	}
	
}
