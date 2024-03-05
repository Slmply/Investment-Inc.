using Godot;
using System;

public partial class Throwable : CharacterBody2D
{

	private bool held = false;


    public override void _Ready()
    {
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
		

		if (held) {

		} else {
			
		}
    }

}
