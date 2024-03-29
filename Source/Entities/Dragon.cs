using System;
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

	private AudioStreamPlayer2D AudioStreamPlayer2D;

	private Array<Node> Links;
	
	private double extendAlapsed;

	private double curExtend;

	public bool Extended { get { return this.extended; } }
	private bool extended = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Dragon Instance ID: ", GetInstanceId());
		GD.Print("Dragon Hash Code: ", GetHashCode());
		this.Player = this.GetParent().GetNode<player>("PlayerCharacter");
		this.DragonHead = this.GetNode<Node2D>("DragonHead");
		this.DragonNeck = this.GetNode<Node2D>("DragonNeck");
		this.DragonBody = this.GetNode<DragonBody>("DragonBody");
		this.RootHeadPos = this.DragonHead.GlobalPosition;
		this.RootNeckPos = this.DragonNeck.GlobalPosition;
		this.RootDragonPos = this.DragonBody.GlobalPosition;
		this.Links = this.GetNode<Node2D>("Links").GetChildren();
		this.AudioStreamPlayer2D = this.GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

		((PinJoint2D)this.Links[0]).NodeA = this.DragonNeck.GetChild<RigidBody2D>(0).GetPath();
		((PinJoint2D)this.Links[0]).NodeB = this.DragonBody.GetNode<RigidBody2D>("RigidBody2D5").GetPath();
		((PinJoint2D)this.Links[1]).NodeA = this.DragonHead.GetChild<RigidBody2D>(0).GetPath();
		((PinJoint2D)this.Links[1]).NodeB = this.DragonNeck.GetChild<RigidBody2D>(0).GetPath();
		((PinJoint2D)this.Links[2]).NodeA = this.DragonHead.GetChild<RigidBody2D>(0).GetPath();
		((PinJoint2D)this.Links[2]).NodeB = this.DragonNeck.GetChild<RigidBody2D>(0).GetPath();

		this.extendAlapsed = 0.0;
		this.curExtend = 0.0;
		this.extended = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.extendAlapsed += delta;
		this.TrackPlayer();

		if (!this.Extended && this.Player.GlobalPosition.DistanceTo(this.RootDragonPos) < 200f)
		{
			this.ExtendNeck();
		}
		else if (this.Player.GlobalPosition.DistanceTo(this.RootDragonPos) > 200f)
		{
			this.RetractNeck();
		}
	}

	public void Expand(float amount)
	{
		this.RootNeckPos = new Vector2(this.RootNeckPos.X, this.RootNeckPos.Y - this.RootNeckPos.Y * 0.2f);

		foreach (Node2D child in this.DragonHead.GetChild<RigidBody2D>(0).GetChildren())
		{
			child.Scale *= amount;
			
			// Adjusting for terrible polygon placement B)
			if (child.Name.Equals("Polygon2D"))
			{
				child.Position *= amount;
			}
		}
		this.DragonNeck.Scale *= amount;
	}
	
	private void TrackPlayer()
	{
		this.DragonNeck.LookAt(this.Player.GlobalPosition);
		this.DragonNeck.RotationDegrees += 180;
	}

	private void ExtendNeck()
	{
		if (!this.AudioStreamPlayer2D.Playing)
		{
			this.AudioStreamPlayer2D.Play();
		}
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
		if (this.AudioStreamPlayer2D.Playing)
		{
			this.AudioStreamPlayer2D.Stop();
		}
		if (this.Extended)
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

	public void EatPlayer(Func<bool> func, double delta)
	{
		this.DragonHead.GetChild<RigidBody2D>(0).GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
		this.DragonNeck.GlobalPosition += (this.Player.GlobalPosition - this.DragonNeck.GlobalPosition).Normalized()*(float)delta*400;
		if (this.DragonNeck.GlobalPosition.DistanceTo(this.Player.GlobalPosition) < 50f)
		{
			func();
		}
	}

}
