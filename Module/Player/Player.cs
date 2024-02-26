using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float WalkSpeed = 350.0f;
	public const float AccelSpeed = 100.0f;
	public const float DecelSpeed = 100.0f;
	public const float AccelMultiplier = 25.0f;
	public const float DodgeSpeed = 800.0f;

	public Timer dodgeTimer = null;

	public override void _Ready()
	{
		dodgeTimer = (Timer)GetNode("DodgeTimer");
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


		if (Input.IsActionJustPressed("Dodge") && dodgeTimer.IsStopped())
		{
			velocity.X = direction.X * DodgeSpeed;
			velocity.Y = direction.Y * DodgeSpeed;
			dodgeTimer.Start();
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}