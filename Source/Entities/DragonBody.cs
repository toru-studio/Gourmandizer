using Godot;
using System;
using System.Collections.Generic;

public partial class DragonBody : Node2D
{
	private Skeleton2D Skeleton2D;
	private Polygon2D Sprite2D;
	private List<RigidBody2D> RigidBodies;
	private List<Vector2> RigidBodyPositions;

	private double alapsed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Skeleton2D = this.GetNode<Skeleton2D>("Skeleton2D");
		this.Sprite2D = this.GetNode<Polygon2D>("Sprite2D");
		this.RigidBodies = new List<RigidBody2D>();
		this.RigidBodyPositions = new List<Vector2>();
		
		for (int i = 0; i < this.Skeleton2D.GetChildren().Count; i++)
		{
			RigidBodies.Add(this.GetNode<RigidBody2D>("RigidBody2D" + i));
			this.RigidBodyPositions.Add(this.ToLocal(RigidBodies[i].GlobalPosition));
		}

		this.alapsed = 0.0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		alapsed += delta;
		if (alapsed > 3.0)
		{
			this.Expand(1.1f);
			alapsed = 0;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		for (int i = 0; i < this.Skeleton2D.GetChildren().Count; i++)
		{
			this.Skeleton2D.GetNode<Bone2D>(Convert.ToString(i)).GlobalPosition = this.RigidBodies[i].GlobalPosition;
			
			if (i == 0)
			{
				continue;
			}
			
			this.Skeleton2D.GetNode<Bone2D>(Convert.ToString(i)).LookAt(this.RigidBodies[i].GlobalPosition);
		}
	}

	public void Expand(float amount)
	{
		float minHeight = float.MaxValue;
		float maxHeight = float.MinValue;

		foreach (Bone2D bone2D in this.Skeleton2D.GetChildren())
		{
			bone2D.Scale *= amount;
		}
		
		for (int i = 0; i < this.RigidBodies.Count; i++)
		{
			RigidBody2D rigidBody2D = this.RigidBodies[i];
			
			this.RigidBodyPositions[i] *= amount;

			rigidBody2D.GetChild<CollisionShape2D>(0).Scale *= amount;

			rigidBody2D.GlobalPosition += this.RigidBodyPositions[i];

			if (this.RigidBodyPositions[i].Y < minHeight)
			{
				minHeight = rigidBody2D.GlobalPosition.Y;
			}

			if (this.RigidBodyPositions[i].Y > maxHeight)
			{
				maxHeight = rigidBody2D.GlobalPosition.Y;
			}

			if (rigidBody2D.Name.Equals("RigidBody2D0"))
			{
				continue;
			}

			for (int j = 1; j < 4; j++)
			{
				DampedSpringJoint2D dampedSpringJoint2D = rigidBody2D.GetChild<DampedSpringJoint2D>(j);
				dampedSpringJoint2D.Length *= amount;
				dampedSpringJoint2D.RestLength = dampedSpringJoint2D.Length;
			}
		}

		this.GlobalPosition = new Vector2(this.GlobalPosition.X, this.GlobalPosition.Y - (maxHeight - minHeight) * 4);
	}
}
