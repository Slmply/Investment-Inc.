using Godot;
using System;

public enum EnemyState {
	IDLE,
	FOLLOW,
	CIRCLE,
	ATTACK,
	HIT
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
	public float hitCircleRadius = 100f;
	[Export]
	public EnemyState state = EnemyState.IDLE;

	public EnemyState State {
		get {
			return state;
		}
		set {
			switch (value) {
				case EnemyState.IDLE:
					reRollRandom();
				break;
				case EnemyState.FOLLOW:
					reRollRandom();
				break;
				case EnemyState.CIRCLE:
					reRollRandom();
					if (attackTimer == null) {
						attackTimer = new Timer();
						AddChild(attackTimer);
					}
					attackTimer.OneShot = true;
					attackTimer.WaitTime = GD.RandRange(3, 10);
					attackTimer.Timeout += enterAttack;
					attackTimer.Start();
				break;
				case EnemyState.ATTACK:
					reRollRandom();
				break;
				case EnemyState.HIT:
					attack();
				break;
			}
			state = value;
		}
	}
	[Export]
	public Area2D aggroArea = null;
	[Export]
	public Area2D deAggroArea = null;

	private Player targetPlayer = null;

	private double random = 0f;
	private Timer attackTimer = null;

    public override void _Ready()
    {
        aggroArea.BodyEntered += AggroEntered;
		deAggroArea.BodyExited += DeAggroExited;
    }

    public override void _PhysicsProcess(double delta)
    {
        switch(state) {
			case EnemyState.IDLE:

			break;
			case EnemyState.FOLLOW:
				move(targetPlayer.GlobalPosition, (float)delta);
				if (targetPlayer.GlobalPosition.DistanceTo(GlobalPosition) <= circleRadius) {
					State = EnemyState.CIRCLE;
				}
			break;
			case EnemyState.CIRCLE:
				move(CirclePosition(targetPlayer.GlobalPosition, circleRadius), (float)delta);
			break;
			case EnemyState.ATTACK:	
				move(targetPlayer.GlobalPosition, (float)delta);
				if (targetPlayer.GlobalPosition.DistanceTo(GlobalPosition) <= hitCircleRadius) {
					State = EnemyState.HIT;
					GD.Print("AHHHHH");
				}
			break;
		}
    }

	public virtual void attack() {

	}

    protected void move(Vector2 target, float delta) {
		Vector2 direction = (target - GlobalPosition).Normalized();
		Vector2 velocityDesired = direction * speed;
		Velocity += (velocityDesired - Velocity) * delta * 1.5f;

		MoveAndSlide();
	}

	public void enterAttack() {
		if (state == EnemyState.CIRCLE) {
			State = EnemyState.ATTACK;
		}
		
	}

	public void AggroEntered(Node2D node) {

		if (node is Player) {
			targetPlayer = (Player)node;
			State = EnemyState.FOLLOW;
		}
	}

	public void DeAggroExited(Node2D node) {

		if (targetPlayer != null && node is Player) {
			targetPlayer = null;
			State = EnemyState.IDLE;
		}
	}

	public void reRollRandom() {
		Random rand = new Random();
		random = rand.NextDouble();
	}

	public Vector2 CirclePosition(Vector2 center, float radius) {

		double angle = 2 * Math.PI * random;
		Vector2 offset = Vector2.Zero;

		offset.X = (float)Math.Cos(angle) * radius;
		offset.Y = (float)Math.Sin(angle) * radius;

		return offset + center;
	}

}
