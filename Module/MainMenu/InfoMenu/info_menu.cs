using Godot;
using System;

public partial class info_menu : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Exit() {
		GetParent().GetParent<SceneManager>().transitionScenes(GD.Load<PackedScene>("res://Module/MainMenu/MainMenu.tscn"));
	}
}
