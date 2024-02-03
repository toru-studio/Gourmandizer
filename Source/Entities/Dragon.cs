using Godot;
using Godot.Collections;

public partial class Dragon : Node2D
{
	private Node2D DragonHead;

	private Node2D DragonNeck;

	private Node2D DragonBody;

	private Array<Node> Links;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.DragonHead = this.GetNode<Node2D>("DragonHead");
		this.DragonNeck = this.GetNode<Node2D>("DragonNeck");
		this.DragonBody = this.GetNode<Node2D>("DragonBody");
		this.Links = this.GetNode<Node2D>("Links").GetChildren();

		((PinJoint2D)this.Links[0]).NodeA = this.DragonHead.GetChild<RigidBody2D>(0).GetPath();
		((PinJoint2D)this.Links[0]).NodeB = this.DragonNeck.GetChild<RigidBody2D>(0).GetPath();
		((PinJoint2D)this.Links[1]).NodeA = this.DragonNeck.GetChild<RigidBody2D>(0).GetPath();
		((PinJoint2D)this.Links[1]).NodeB = this.DragonBody.GetNode<RigidBody2D>("RigidBody2D5").GetPath();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
