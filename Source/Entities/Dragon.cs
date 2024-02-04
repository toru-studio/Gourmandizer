using Godot;
using Godot.Collections;

public partial class Dragon : Node2D
{
	private Node2D DragonHead;

	private Node2D DragonNeck;

	private DragonBody DragonBody;

	private Array<Node> Links;
	
	private double alapsed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.DragonHead = this.GetNode<Node2D>("DragonHead");
		this.DragonNeck = this.GetNode<Node2D>("DragonNeck");
		this.DragonBody = this.GetNode<DragonBody>("DragonBody");
		this.Links = this.GetNode<Node2D>("Links").GetChildren();

		((PinJoint2D)this.Links[0]).NodeA = this.DragonNeck.GetChild<RigidBody2D>(0).GetPath();
		((PinJoint2D)this.Links[0]).NodeB = this.DragonBody.GetNode<RigidBody2D>("RigidBody2D5").GetPath();
		
		this.alapsed = 0.0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		alapsed += delta;
		if (alapsed > 3.0)
		{
			DragonBody.Expand(1.1f);
			this.DragonHead.Scale *= 1.1f;
			this.DragonHead.GlobalPosition = new Vector2(this.DragonHead.GlobalPosition.X, this.DragonHead.GlobalPosition.Y - this.DragonHead.GlobalPosition.Y * 0.1f);
			this.DragonNeck.Scale *= 1.1f;
			this.DragonNeck.GlobalPosition = new Vector2(this.DragonNeck.GlobalPosition.X, this.DragonNeck.GlobalPosition.Y - this.DragonNeck.GlobalPosition.Y * 0.1f);
			foreach (PinJoint2D pinJoint2D in this.Links)
			{
				pinJoint2D.Scale *= 1.1f;
				pinJoint2D.GlobalPosition = new Vector2(pinJoint2D.GlobalPosition.X, pinJoint2D.GlobalPosition.Y - pinJoint2D.GlobalPosition.Y * 0.1f);
			}
			alapsed = 0.0f;
		}
	}
}
