using Godot;
using System;

public partial class BankTeller : Enemy
{

    private AnimationPlayer animPlayer;
    [Export]
    public float attackDistance = 250f;

    public void attackDamage() {
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
        animPlayer.Play("Death");

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

        sprt.FlipH = (Velocity.X != 0)? ((Velocity.X > 0)? false : true) : sprt.FlipH;

        if (state == EnemyState.DEATH) {
            sprt.FlipH = !sprt.FlipH;
        }
        
    }
}
