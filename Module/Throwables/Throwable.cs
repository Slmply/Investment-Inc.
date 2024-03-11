using Godot;
using System;

public partial class Throwable : CharacterBody2D
{

  [Export]
  public float throwSpeed = 1500f;
  public Player holder;
  private Area2D hitArea;



  public override void _Ready()
  {
    base._Ready();

    hitArea = GetNode<Area2D>("HitArea");
  }

  public override void _PhysicsProcess(double delta)
  {

    if (holder != null)
    {
      this.GlobalPosition = holder.holdPoint.GlobalPosition;
    }
    else
    {
      Velocity = Lerp(Velocity, Vector2.Zero, (float)delta * 2.5f);
      MoveAndSlide();
    }

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

  public void lob(Vector2 lobDir)
  {
    holder = null;
    Velocity = lobDir * throwSpeed;
    // GD.Print(Velocity);
  }

  public void HitAreaEntered(Node2D body)
  {

    if (body is Enemy && Velocity.Length() >= 200)
    {
      Enemy enemy = (Enemy)body;

      if (enemy.State != EnemyState.DEATH)
      {
        enemy.State = EnemyState.DEATH;
        enemy.Velocity = (-1 * (GlobalPosition - enemy.GlobalPosition).Normalized()) * Velocity.Length();
      }
    }

  }

}
