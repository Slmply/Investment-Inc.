using Godot;
using System;

public partial class BankTeller : Enemy
{
    public override void attack()
    {
		
    }

    public override void animate()
    {
        AnimationPlayer animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

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
        }
    }
}
