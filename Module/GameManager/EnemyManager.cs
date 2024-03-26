using Godot;
using System;
using System.Linq;

public partial class EnemyManager : Node2D
{

	[Export]
	public GameManager gm;
	public Node2D spawnPointContainer;
	public PackedScene bankTellerScene;
	[Export]
	public float enemySpawnTimeMin = 0.5f;
	[Export]
	public float enemySpawnTimeMax = 0.25f;
	[Export]
	public int enemyMaxGroupSpawnAmount = 6;
	[Export]
	public int maxEnemyCount = 15;

	private float nextSpawnTime = 0f;

	public void updateSpawns(Player player, float time) {
		if (time >= nextSpawnTime) {
			for (int i = 0; i < GD.RandRange(0, enemyMaxGroupSpawnAmount); i++) {
				spawnEnemy(getBestSpawnPoint(player.GlobalPosition));
			}

			nextSpawnTime = time + (float)GD.RandRange((double)enemySpawnTimeMin, (double)enemySpawnTimeMax);
		}
	}


    public override void _Ready()
    {
		bankTellerScene = GD.Load<PackedScene>("res://Module/Enemies/BankTeller/bank_teller_enemy.tscn");
		nextSpawnTime = (float)GD.RandRange((double)enemySpawnTimeMin, (double)enemySpawnTimeMax);
    }

    public void spawnEnemy (Node2D spawnPoint) {

		if (GetChildCount() <= maxEnemyCount) {
			BankTeller enemy = (BankTeller) bankTellerScene.Instantiate();

			AddChild(enemy);
			enemy.EnemyDeath += onDeath;

			enemy.GlobalPosition = spawnPoint.GlobalPosition;
		}

		

		// GD.Print("Enemy Spawned");
	}

	public void onDeath() {
		gm.money += 10;
		gm.enemyDefeatCount++;
	}

	public Node2D getBestSpawnPoint(Vector2 playerPosition) {
		Godot.Collections.Array<Node> points = spawnPointContainer.GetChildren();
	

		Godot.Collections.Array<Node2D> validPoints = new Godot.Collections.Array<Node2D>();
		
		

		foreach (Node n in points) {
			Node2D node = (Node2D) n;

			if (node.GlobalPosition.DistanceTo(playerPosition) >= 500) {
				// GD.Print("Adding");
				validPoints.Add(node);
			}

		}
		
		
		return validPoints.PickRandom();
	}
}
