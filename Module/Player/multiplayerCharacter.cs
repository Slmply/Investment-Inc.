using Godot;
using System;
using System.Linq;

public partial class multiplayerCharacter : Player
{
	// [Export]
	// public float WalkSpeed = 350.0f;
	// [Export]
	// public float AccelSpeed = 100.0f;
	// [Export]
	// public float DecelSpeed = 100.0f;
	// [Export]
	// public float AccelMultiplier = 25.0f;
	// [Export]
	// public float DodgeSpeed = 800.0f;
	// public AnimationPlayer anmPlayer;
	// private Area2D pickupArea;
	// private Throwable heldItem;
	// [Export]
	// public Node2D holdPoint;

	// [Signal]
	// public delegate void onHitEventHandler();

	[Export]
	public Player owner = null;

	public override void _Ready()
	{
		anmPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		pickupArea = GetNode<Area2D>("PickupArea");
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("mp_lob"))
		{


			if (heldItem == null)
			{
				Node2D[] items = pickupArea.GetOverlappingBodies().ToArray<Node2D>();

				foreach (Node2D n in items)
				{

					if (n is Throwable)
					{
						Throwable i = (Throwable)n;
						if (i.holder == null)
						{
							heldItem = i;

							i.holder = this;
							break;
						}
					}
				}
			}
			else
			{
				heldItem.lob(Velocity.Normalized());
				heldItem = null;
			}



		}
	}

	// public void hit() {
	// 	if (!stunned && dodgeTimer.IsStopped() && iTimer.IsStopped()) {
	// 		stunned = true;
	// 		anmPlayer.Play("Hit");
	// 		EmitSignal(SignalName.onHit);

	// 		if (heldItem != null) {
	// 			heldItem.drop(Velocity.Normalized());
	// 			heldItem = null;
	// 		}
	// 	}
	// }

	public void animFinished(string anim)
	{
		// if (anim == "Hit") {
		// 	stunned = false;
		// 	iTimer.Start();
		// }
	}



	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.

		Vector2 direction = Input.GetVector("mp_left", "mp_right", "mp_up", "mp_down");
		direction = direction.Normalized();

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



		// if (Input.IsActionJustPressed("Dodge") && dodgeTimer.IsStopped() && Velocity.Length() >= 0.5f)
		// {
		// 	velocity.X = direction.X * DodgeSpeed;
		// 	velocity.Y = direction.Y * DodgeSpeed;
		// 	dodgeTimer.Start();
		// 	GetNode<AudioStreamPlayer>("DodgeSound").Play();

		// 	if (heldItem != null) {
		// 		heldItem.drop(Velocity.Normalized());
		// 		heldItem = null;
		// 	}
		// }



		if (anmPlayer.CurrentAnimation == "Roll")
		{
			anmPlayer.Play("Roll");
		}
		else if (Velocity.Length() > 0.5)
		{
			anmPlayer.Play("Run");
		}
		else
		{
			anmPlayer.Play("Idle");
		}

		Velocity = velocity;

		GetNode<Sprite2D>("Sprite2D").FlipH = (Velocity.X == 0) ? GetNode<Sprite2D>("Sprite2D").FlipH : (Velocity.X > 0) ? false : true;

		MoveAndSlide();

		if (owner != null)
		{
			if (owner.GlobalPosition.DistanceTo(GlobalPosition) > 600)
			{
				GlobalPosition = new Vector2(
					owner.GlobalPosition.X + (float)Math.Cos(owner.GlobalPosition.AngleToPoint(GlobalPosition)) * 600.0f,
					owner.GlobalPosition.Y + (float)Math.Sin(owner.GlobalPosition.AngleToPoint(GlobalPosition)) * 600.0f
				);

			}
		}
	}
}
