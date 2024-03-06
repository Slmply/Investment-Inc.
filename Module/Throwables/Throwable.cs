using Godot;
using System;

public partial class Throwable : CharacterBody2D
{

	public Player holder;



    public override void _Ready()
    {
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
		
		if (holder != null) {
      this.GlobalPosition = holder.holdPoint.GlobalPosition;
		} else {
			Velocity = Lerp(Velocity, Vector2.Zero, (float)delta);
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

    public void lob(Vector2 newVelocity) {
      holder = null;
      this.Velocity = newVelocity;
    }

}
