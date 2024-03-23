using Godot;
using System;
using System.Collections;
using System.Xml.Schema;

public enum EnemyState
{
	IDLE,
	ALERT,
	FOLLOW,
	CIRCLE,
	ATTACK,
	HIT,
	DEATH
}

public partial class Enemy : CharacterBody2D
{
	[Export]
	public float speed = 250f;
	[Export]
	public float health = 25f;
	[Export]
	public float circleRadius = 300f;
	[Export]
	public float hitCircleRadius = 150f;
	[Export]
	public EnemyState state = EnemyState.IDLE;
	public EnemyState State
	{
		get
		{
			return state;
		}
		set
		{
			switch (value)
			{
				case EnemyState.IDLE:
					reRollRandom();
					state = value;
					break;
				case EnemyState.ALERT:
					reRollRandom();
					state = value;
					break;
				case EnemyState.FOLLOW:
					reRollRandom();
					state = value;
					break;
				case EnemyState.CIRCLE:
					reRollRandom();
					if (attackTimer == null)
					{
						attackTimer = new Timer();
						AddChild(attackTimer);
					}
					attackTimer.OneShot = true;
					attackTimer.WaitTime = GD.RandRange(3, 10);
					attackTimer.Timeout += enterAttack;
					attackTimer.Start();
					state = value;
					break;
				case EnemyState.ATTACK:
					reRollRandom();
					state = value;
					break;
				case EnemyState.HIT:
					reRollRandom();
					attack();
					state = value;
					break;
				case EnemyState.DEATH:
					onDeath((targetPlayer.GlobalPosition - GlobalPosition).Normalized() * 600f);
					state = value;
					break;
			}

		}
	}
	[Export]
	public Area2D aggroArea = null;
	[Export]
	public Area2D deAggroArea = null;
	protected Player targetPlayer = null;
	private double random = 0f;
	private Timer attackTimer = null;

	public override void _Ready()
	{
		aggroArea.BodyEntered += AggroEntered;
		deAggroArea.BodyExited += DeAggroExited;
	}

	public override void _PhysicsProcess(double delta)
	{
		GD.Print(state);
		switch (state)
		{
			case EnemyState.IDLE:
				move(GlobalPosition, (float)delta);
				break;
			case EnemyState.ALERT:
				move(GlobalPosition, (float)delta);
				RayCast2D visionRay = GetNode<RayCast2D>("VisibilityCast");
				visionRay.TargetPosition = targetPlayer.GlobalPosition;
				if (!(visionRay.IsColliding())) {
					State = EnemyState.FOLLOW;
				}
				break;
			case EnemyState.FOLLOW:
				move(targetPlayer.GlobalPosition, (float)delta);
				if (targetPlayer.GlobalPosition.DistanceTo(GlobalPosition) <= circleRadius)
				{
					State = EnemyState.CIRCLE;
				}
				break;
			case EnemyState.CIRCLE:
				move(CirclePosition(targetPlayer.GlobalPosition, circleRadius), (float)delta);
				break;
			case EnemyState.ATTACK:
				move(targetPlayer.GlobalPosition, (float)delta);
				if (targetPlayer.GlobalPosition.DistanceTo(GlobalPosition) <= hitCircleRadius)
				{
					State = EnemyState.HIT;
				}
				break;
			case EnemyState.HIT:
				move(GlobalPosition, (float)delta);
				break;
			case EnemyState.DEATH:
				Velocity = Velocity = Lerp(Velocity, Vector2.Zero, (float)delta * 2f);
				MoveAndSlide();
				break;
		}

		animate();
	}

	public virtual void onDeath(Vector2 launchVector)
	{
		// Velocity = launchVector;

	}

	public virtual void attack()
	{

	}

	public virtual void animate()
	{

	}

	protected void move(Vector2 target, float delta, float margin = 40f)
	{

		if (target.DistanceTo(GlobalPosition) <= margin)
		{
			Velocity = Lerp(Velocity, Vector2.Zero, delta * 2.5f);
		}
		else
		{
			Vector2 direction = (target - GlobalPosition).Normalized();
			Vector2 velocityDesired = direction * speed;
			Velocity += (velocityDesired - Velocity) * delta * 2.5f;
		}

		MoveAndSlide();
	}

	Vector2 Lerp(Vector2 firstVector, Vector2 secondVector, float by)
	{
		float retX = Lerp(firstVector.X, secondVector.X, by);
		float retY = Lerp(firstVector.Y, secondVector.Y, by);
		return new Vector2(retX, retY);
	}
	float Lerp(float firstFloat, float secondFloat, float by)
	{
		return firstFloat * (1 - by) + secondFloat * by;
	}

	public void enterAttack()
	{
		if (state == EnemyState.CIRCLE)
		{
			State = EnemyState.ATTACK;
		}
	}

	public void AggroEntered(Node2D node)
	{

		if (node is Player && targetPlayer == null)
		{
			GD.Print("Aggrod");
			targetPlayer = (Player)node;
			State = EnemyState.ALERT;
		}
	}

	public void DeAggroExited(Node2D node)
	{

		if (node is Player)
		{
			GD.Print("DeAggrod");
			State = EnemyState.IDLE;
			targetPlayer = null;
		}
	}

	public void reRollRandom()
	{
		Random rand = new Random();
		random = rand.NextDouble();
	}

	public Vector2 CirclePosition(Vector2 center, float radius)
	{

		double angle = 2 * Math.PI * random;
		Vector2 offset = Vector2.Zero;

		offset.X = (float)Math.Cos(angle) * radius;
		offset.Y = (float)Math.Sin(angle) * radius;

		return offset + center;
	}

	public void destroySelf()
	{
		QueueFree();
	}

}
