using Godot;
using Godot.Collections;

public partial class Dragon : Node2D
{
	private Node2D DragonHead;

	private Node2D DragonNeck;

	private Vector2 RootHeadPos;
	
	private Vector2 RootNeckPos;

	private DragonBody DragonBody;

	private Array<Node> Links;
	
	private double alapsed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.DragonHead = this.GetNode<Node2D>("DragonHead");
		this.DragonNeck = this.GetNode<Node2D>("DragonNeck");
		this.DragonBody = this.GetNode<DragonBody>("DragonBody");
		this.RootHeadPos = this.DragonHead.GlobalPosition;
		this.RootNeckPos = this.DragonNeck.GlobalPosition;
		this.Links = this.GetNode<Node2D>("Links").GetChildren();

		((PinJoint2D)this.Links[0]).NodeA = this.DragonNeck.GetChild<RigidBody2D>(0).GetPath();
		((PinJoint2D)this.Links[0]).NodeB = this.DragonBody.GetNode<RigidBody2D>("RigidBody2D5").GetPath();
		
		this.alapsed = 0.0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		alapsed += delta;
		if (alapsed > 20.0)
		{
			DragonBody.Expand(1.1f);
			this.RootHeadPos = new Vector2(this.RootHeadPos.X, this.RootHeadPos.Y - this.RootHeadPos.Y * 0.1f);
			this.RootNeckPos = new Vector2(this.RootNeckPos.X, this.RootNeckPos.Y - this.RootNeckPos.Y * 0.1f);
			this.DragonHead.Scale *= 1.1f;
			this.DragonHead.GlobalPosition = this.RootHeadPos;
			this.DragonNeck.Scale *= 1.1f;
			this.DragonNeck.GlobalPosition = this.RootNeckPos;
			foreach (PinJoint2D pinJoint2D in this.Links)
			{
				pinJoint2D.Scale *= 1.1f;
				pinJoint2D.GlobalPosition = new Vector2(pinJoint2D.GlobalPosition.X, pinJoint2D.GlobalPosition.Y - pinJoint2D.GlobalPosition.Y * 0.1f);
			}
			alapsed = 0.0f;
		}
	}
}
