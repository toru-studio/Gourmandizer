using Godot;
using System;

public partial class Food : Area2D
{
	public player Player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Player = (player)GetParent().GetParent().GetChild(1);

	}
	

	private void _on_body_entered(Node2D body)
	{
		Console.WriteLine(body.Name);

		
		if (body.Name == "PlayerCharacter") ;
		{
			if (body.Name != "PlayerCharacter") return;
			Player.FlowerSound.Play();
			Player.FoodItems += 1;
			Console.WriteLine("Weight is Now " + Player.CurrentWeight + 1);
			QueueFree();
		}
	}
}
