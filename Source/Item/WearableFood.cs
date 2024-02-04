using Godot;
using System;

public partial class WearableFood : RigidBody2D
{
	private Sprite2D _sprite2D = new Sprite2D();
	private CollisionShape2D _collisionShape2D = new CollisionShape2D();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		var temp = new CircleShape2D();
		temp.Radius = 4;
		_collisionShape2D.Shape = temp;
		_sprite2D.load
	}

}
