using Godot;
using System;

public partial class BankTeller : Enemy
{

    private AnimationPlayer animPlayer;
    [Export]
    public float attackDistance = 250f;

    [Signal]
    public delegate void EnemyDeathEventHandler();

    public void attackDamage() {
        GetNode<AudioStreamPlayer>("AttackSound").Play();
        if (targetPlayer.Position.DistanceTo(GlobalPosition) <= attackDistance) {
            targetPlayer.hit();
        }
    }

    public override void _Ready()
    {
        base._Ready();
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void attack()
    {
        animPlayer.Play("Attack");
    }

    public override void onDeath(Vector2 launchVector)
    {
        base.onDeath(launchVector);
        GetNode<AudioStreamPlayer>("HitSound").Play();
        animPlayer.Play("Death");
        EmitSignal(SignalName.EnemyDeath);

    }

    public void AnimFinished(string anim) {
        if (anim == "Death") {
            destroySelf();
        } else if (anim == "Attack") {


            State = EnemyState.CIRCLE;
        }
    }

    public override void animate()
    {
        switch (state) {
            case EnemyState.IDLE:
            case EnemyState.FOLLOW:
            case EnemyState.CIRCLE:
            case EnemyState.ATTACK:

            if (Velocity.Length() > 20) {
                animPlayer.Play("Run");
            } else {
                animPlayer.Play("Idle");
            }

            break;
            case EnemyState.HIT:

            break;
            case EnemyState.DEATH:
            break;
        }

        Sprite2D sprt = GetNode<Sprite2D>("Sprite2D");

        if (Velocity.X != 0) {
            if (Velocity.X > 10) {
                sprt.FlipH = false;
            } else if (Velocity.X < 10) {
                sprt.FlipH = true;
            }
        }

        sprt.FlipH = (Velocity.X != 0)? ((Velocity.X > 10)? false : true) : sprt.FlipH;

        if (state == EnemyState.DEATH && Math.Abs(Velocity.X) > 10) {
            sprt.FlipH = !sprt.FlipH;
        }
        
    }
}
