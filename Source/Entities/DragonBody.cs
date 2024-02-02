using Godot;
using System;
using System.Collections.Generic;

public partial class DragonBody : Node
{
	private Skeleton2D Skeleton2D;

	private List<RigidBody2D> RigidBodies;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Skeleton2D = this.GetNode<Skeleton2D>("Skeleton2D");
		RigidBodies = new List<RigidBody2D>();
		for (int i = 0; i < this.Skeleton2D.GetChildren().Count; i++)
		{
			RigidBodies.Add(this.GetNode<RigidBody2D>("RigidBody2D" + i));
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		for (int i = 0; i < this.Skeleton2D.GetChildren().Count; i++)
		{
			this.Skeleton2D.GetNode<Bone2D>(Convert.ToString(i)).GlobalPosition = RigidBodies[i].GlobalPosition;
		}
	}
}
