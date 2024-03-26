using Godot;
using System;

public partial class MainMenu : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<AnimationPlayer>("Sprite2D/AnimationPlayer").Play("Run");
		GetNode<AnimationPlayer>("MenuAnim").Play("MenuAnim");
	}


	public void Play() {
		GetParent().GetParent<SceneManager>().transitionScenes(GD.Load<PackedScene>("res://Module/World.tscn"));
	}
	
	public void Info() {
		GetParent().GetParent<SceneManager>().transitionScenes(GD.Load<PackedScene>("res://Module/MainMenu/InfoMenu/info_menu.tscn"));
	}

	public void Quit() {
		GetParent().GetParent<SceneManager>().dummyTransition(() => {GetTree().Quit(); return 0;});
	}
}
