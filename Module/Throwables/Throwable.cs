using Godot;
using System;

public partial class Throwable : CharacterBody2D
{

  [Export]
  public float throwSpeed = 1500f;
  [Export]
  public float rotationAmount = 1f;
  public Player holder;
  private Area2D hitArea;
  private Sprite2D sprite;
  



  public override void _Ready()
  {
	base._Ready();

	hitArea = GetNode<Area2D>("HitArea");

	sprite = GetNode<Sprite2D>("Sprite2D");
	int rand = GD.RandRange(0, sprite.Vframes - 1);
	sprite.FrameCoords = new Vector2I(0, rand);
  }

  public override void _PhysicsProcess(double delta)
  {

	if (holder != null)
	{
	  this.GlobalPosition = holder.holdPoint.GlobalPosition;
	  sprite.RotationDegrees = 0;
	}
	else
	{
	  Velocity = Lerp(Velocity, Vector2.Zero, (float)delta * 2.5f);
	  if (Velocity.Length() > 0) {
		sprite.RotationDegrees += (Velocity.X / throwSpeed) * rotationAmount;
	  }
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
	GetNode<AudioStreamPlayer>("ThrowSFX").Play();
	// GD.Print(Velocity);
  }

  public void drop(Vector2 lobDir) {
	holder = null;
	Velocity = lobDir * 100f;
	// GD.Print(Velocity);
  }

  public void HitAreaEntered(Node2D body)
  {

	if (body is Enemy && Velocity.Length() >= 200 && holder == null)
	{
	  Enemy enemy = (Enemy)body;

	  if (enemy.State != EnemyState.DEATH)
	  {
		enemy.State = EnemyState.DEATH;
		enemy.Velocity = (-1 * (GlobalPosition - enemy.GlobalPosition).Normalized()) * Velocity.Length();
		Velocity = (-1 * Velocity) / 2;
	  }
	}

  }

}
