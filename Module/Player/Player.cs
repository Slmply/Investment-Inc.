using Godot;
using System;
using System.Linq;

public partial class Player : CharacterBody2D
{
	[Export]
	public float WalkSpeed = 350.0f;
	[Export]
	public float AccelSpeed = 100.0f;
	[Export]
	public float DecelSpeed = 100.0f;
	[Export]
	public float AccelMultiplier = 25.0f;
	[Export]
	public float DodgeSpeed = 800.0f;

	public Timer dodgeTimer = null;
	public AnimationPlayer anmPlayer;
	private Area2D pickupArea;
	private Throwable heldItem;
	[Export]
	public Node2D holdPoint;

	public override void _Ready()
	{
		dodgeTimer = (Timer)GetNode("DodgeTimer");
		anmPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		pickupArea = GetNode<Area2D>("PickupArea");
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("Interact")  && dodgeTimer.IsStopped()) {

			GD.Print("Interacted");

			if (heldItem == null) {
				Node2D[] items = pickupArea.GetOverlappingBodies().ToArray<Node2D>();

				foreach (Node2D n in items) {
					if (n is Throwable) {
						heldItem = (Throwable)n;
						((Throwable)n).holder = this;
						break;
					}
				}
			} else {
				heldItem.lob(Velocity.Normalized() * 750);
				heldItem = null;
			}
			
			

		}
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.

		Vector2 direction = Input.GetVector("Move_Left", "Move_Right", "Move_Up", "Move_Down");
		direction = direction.Normalized();
		if (dodgeTimer.IsStopped())
		{
			if (direction != Vector2.Zero)
			{
				velocity.X = Mathf.MoveToward(Velocity.X, direction.X * WalkSpeed, AccelSpeed * (float)delta * AccelMultiplier);
				velocity.Y = Mathf.MoveToward(Velocity.Y, direction.Y * WalkSpeed, AccelSpeed * (float)delta * AccelMultiplier);
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, DecelSpeed * (float)delta * AccelMultiplier);
				velocity.Y = Mathf.MoveToward(Velocity.Y, 0, DecelSpeed * (float)delta * AccelMultiplier);
			}
		}
		else
		{
			if (direction != Vector2.Zero)
			{
				velocity.X = Mathf.MoveToward(Velocity.X, direction.X * WalkSpeed, AccelSpeed * (float)delta * AccelMultiplier / 2.0f);
				velocity.Y = Mathf.MoveToward(Velocity.Y, direction.Y * WalkSpeed, AccelSpeed * (float)delta * AccelMultiplier / 2.0f);
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, DecelSpeed * (float)delta * AccelMultiplier / 2.0f);
				velocity.Y = Mathf.MoveToward(Velocity.Y, 0, DecelSpeed * (float)delta * AccelMultiplier / 2.0f);
			}
		}


		if (Input.IsActionJustPressed("Dodge") && dodgeTimer.IsStopped() && Velocity.Length() >= 0.5f)
		{
			velocity.X = direction.X * DodgeSpeed;
			velocity.Y = direction.Y * DodgeSpeed;
			dodgeTimer.Start();
		}



		if (!dodgeTimer.IsStopped() || anmPlayer.CurrentAnimation == "Roll") {
			anmPlayer.Play("Roll");
		} else if (Velocity.Length() > 0.5) {
			anmPlayer.Play("Run");
		} else {
			anmPlayer.Play("Idle");
		}

		Velocity = velocity;

		GetNode<Sprite2D>("Sprite2D").FlipH = (Velocity.X == 0)? GetNode<Sprite2D>("Sprite2D").FlipH : (Velocity.X > 0)?  false : true; 

		MoveAndSlide();
	}
}
