using Godot;
using System;
using System.Collections.Generic;

public partial class DragonBody : Node2D
{
	private Skeleton2D Skeleton2D;
	private Polygon2D Sprite2D;
	private List<RigidBody2D> RigidBodies;

	private double alapsed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Skeleton2D = this.GetNode<Skeleton2D>("Skeleton2D");
		this.Sprite2D = this.GetNode<Polygon2D>("Sprite2D");
		this.RigidBodies = new List<RigidBody2D>();
		
		for (int i = 0; i < this.Skeleton2D.GetChildren().Count; i++)
		{
			RigidBodies.Add(this.GetNode<RigidBody2D>("RigidBody2D" + i));
		}

		this.alapsed = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		alapsed += delta;
		if (alapsed > 1.0)
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
		}
	}

	public void Expand(float amount)
	{
		foreach (Bone2D bone in Skeleton2D.GetChildren())
		{
			bone.Position *= amount;
		}

		foreach (RigidBody2D rigidBody2D in RigidBodies)
		{
			rigidBody2D.Position *= amount;

			if (rigidBody2D.Name.Equals("RigidBody2D0"))
			{
				continue;
			}

			rigidBody2D.GetChild<DampedSpringJoint2D>(1).Length *= amount;
			rigidBody2D.GetChild<DampedSpringJoint2D>(2).Length *= amount;
			rigidBody2D.GetChild<DampedSpringJoint2D>(3).Length *= amount;
		}
		
		this.Sprite2D.Scale *= amount;
		this.Position = new Vector2(this.Position.X, this.Position.Y * amount + 2);
	}
}
