using System.Linq;
using Godot;
using Godot.Collections;

public partial class Dragon : Node2D
{
	private player Player;
	
	private Node2D DragonHead;

	private Node2D DragonNeck;

	private Vector2 RootHeadPos;
	
	private Vector2 RootNeckPos;

	private Vector2 RootDragonPos;
	
	private DragonBody DragonBody;

	private Array<Node> Links;
	
	private double expandAlapsed;

	private double extendAlapsed;

	private double curExtend;

	private bool extended;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Player = this.GetParent().GetNode<player>("PlayerCharacter");
		this.DragonHead = this.GetNode<Node2D>("DragonHead");
		this.DragonNeck = this.GetNode<Node2D>("DragonNeck");
		this.DragonBody = this.GetNode<DragonBody>("DragonBody");
		this.RootHeadPos = this.DragonHead.GlobalPosition;
		this.RootNeckPos = this.DragonNeck.GlobalPosition;
		this.RootDragonPos = this.DragonBody.GlobalPosition;
		this.Links = this.GetNode<Node2D>("Links").GetChildren();

		((PinJoint2D)this.Links[0]).NodeA = this.DragonNeck.GetChild<RigidBody2D>(0).GetPath();
		((PinJoint2D)this.Links[0]).NodeB = this.DragonBody.GetNode<RigidBody2D>("RigidBody2D5").GetPath();
		((PinJoint2D)this.Links[1]).NodeA = this.DragonHead.GetChild<RigidBody2D>(0).GetPath();
		((PinJoint2D)this.Links[1]).NodeB = this.DragonNeck.GetChild<RigidBody2D>(0).GetPath();
		((PinJoint2D)this.Links[2]).NodeA = this.DragonHead.GetChild<RigidBody2D>(0).GetPath();
		((PinJoint2D)this.Links[2]).NodeB = this.DragonNeck.GetChild<RigidBody2D>(0).GetPath();

		this.expandAlapsed = 0.0f;
		this.extendAlapsed = 0.0;
		this.curExtend = 0.0;
		this.extended = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.expandAlapsed += delta;
		this.extendAlapsed += delta;
		this.TrackPlayer();

		if (!this.extended && this.Player.GlobalPosition.DistanceTo(this.RootDragonPos) < 200f)
		{
			this.ExtendNeck();
		}
		else if (this.Player.GlobalPosition.DistanceTo(this.RootDragonPos) > 200f)
		{
			this.RetractNeck();
		}

		if (this.expandAlapsed > 20.0)
		{
			this.Expand();
		}
	}

	private void Expand()
	{
		this.RootNeckPos = new Vector2(this.RootNeckPos.X, this.RootNeckPos.Y - this.RootNeckPos.Y * 0.1f);

		foreach (Node2D child in this.DragonHead.GetChild<RigidBody2D>(0).GetChildren())
		{
			child.Scale *= 1.1f;
			
			// Adjusting for terrible polygon placement B)
			if (child.Name.Equals("Polygon2D"))
			{
				child.Position *= 1.1f;
			}
		}
		this.DragonNeck.Scale *= 1.1f;

		foreach (PinJoint2D pinJoint2D in this.Links)
		{
			pinJoint2D.Scale *= 1.1f;
			pinJoint2D.GlobalPosition = new Vector2(pinJoint2D.GlobalPosition.X, pinJoint2D.GlobalPosition.Y - pinJoint2D.GlobalPosition.Y * 0.1f);
		}
		this.expandAlapsed = 0.0;
	}
	private void TrackPlayer()
	{
		this.DragonNeck.LookAt(this.Player.GlobalPosition);
		this.DragonNeck.RotationDegrees += 180;
	}

	private void ExtendNeck()
	{
		if (this.DragonNeck.GlobalPosition.DistanceTo(this.Player.GlobalPosition) < 100f)
		{
			this.extended = true;
		}
		
		if (this.extendAlapsed > .01)
		{
			this.DragonNeck.GlobalPosition +=
				(this.Player.GlobalPosition - this.DragonNeck.GlobalPosition).Normalized();
			this.extendAlapsed = 0.0;
		}
	}

	private void RetractNeck()
	{
		if (this.extended)
		{
			this.extended = false;
		}

		if (this.DragonNeck.GlobalPosition.DistanceTo(this.RootNeckPos) > 2f)
		{
			if (this.extendAlapsed > .01)
			{
				this.DragonNeck.GlobalPosition +=
					(this.RootDragonPos - this.DragonNeck.GlobalPosition).Normalized();
			}
		}
	}
}
