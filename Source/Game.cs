using Godot;
using System;

public partial class Game : Node
{
	private Node2D FoodItems;
	
	private Dragon Dragon;

	private Node2D Credits;

	private AudioStreamPlayer GameOverAudio;
	private AudioStreamPlayer EatenAudio;

	private bool gameOver;
	private bool audioPlayed;

	private double creditTimer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.FoodItems = this.GetNode<Node2D>("FoodItems");
		this.Dragon = this.GetNode<Dragon>("Dragon");
		this.Credits = this.GetNode<Node2D>("Credits");

		this.GameOverAudio = this.GetNode<AudioStreamPlayer>("GameOverAudio");
		this.EatenAudio = this.GetNode<AudioStreamPlayer>("EatenAudio");

		this.gameOver = false;
		this.audioPlayed = false;
		this.creditTimer = 0.0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (gameOver)
		{
			this.GameOver(delta);
			return;
		}
		
		if (this.FoodItems.GetChildren().Count == 0)
		{
			this.Dragon.EatPlayer(this.End, delta);
			
			if (audioPlayed) return;
			this.GameOverAudio.Play();
			this.audioPlayed = true;
		}
		
	}

	public bool End()
	{
		this.EatenAudio.Play();
		this.Credits.GetChild<Sprite2D>(0).Visible = true;

		foreach (Node node in this.GetChildren())
		{
			if (!node.Name.Equals("Credits") && !node.Name.Equals("GameOverAudio") 
			    && !node.Name.Equals("EatenAudio"))
			{
				node.QueueFree();
			}
		}

		this.gameOver = true;

		return true;
	}

	private void GameOver(double delta)
	{
		this.creditTimer += delta;
		if (this.creditTimer > 3.0)
		{
			this.Credits.GetChild<Sprite2D>(1).Visible = true;
		}
		if (this.creditTimer > 5.0)
		{
			this.Credits.GetChild<Sprite2D>(2).Visible = true;
		}
		if (this.creditTimer > 7.0)
		{
			this.Credits.GetChild<Sprite2D>(3).Visible = true;
		}
	}
}
