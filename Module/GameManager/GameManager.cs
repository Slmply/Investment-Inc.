using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class GameManager : Node2D
{

	[Export]
	public float money = 30000;
	[Export]
	public float timeScale = 1;
	public float TimeScale
	{
		get
		{
			return timeScale;
		}
		set
		{
			timeScale = value;
		}
	}

	public float currentTime = 0;

	public StockManager stockManager;
	public EnemyManager enemyManager;
	private Timer gameTick;
	private CanvasLayer activeUi;
	[Export]
	public CanvasLayer ActiveUi {
		get {
			return activeUi;
		}
		set {
			activeUi = value;
			Node2D uiContainer = GetNode<Node2D>("UIContainer");

			foreach (Node n in uiContainer.GetChildren()) {
				if (n is CanvasLayer) {
					((CanvasLayer) n ).Visible = false;
				}
			}

			activeUi.Visible = true;
		}
	}
	[Export]
	public Player player;

	[Signal]
	public delegate void GameCompletionEventHandler(float money, int enemiesKilled);

	public void playerHit() {
		money = money * (float)GD.RandRange(0.9, 0.98);
		exitUi();
	}


	public void resetPlayerPosition() {
		GetParent().GetParent<SceneManager>().dummyTransition(() => {player.Position = new Vector2(760, 350); return 0;});
	}

	public override void _Ready()
	{
		player.onHit += playerHit;
		stockManager = GetNode<StockManager>("Stock Manager");
		enemyManager = GetNode<EnemyManager>("EnemyManager");
		enemyManager.spawnPointContainer = GetParent().GetNode<Node2D>("SpawnPointContainers");
		stockManager.updateStocks(0.0f);
		gameTick = GetNode<Timer>("GameTick");
		gameTick.Start();
		GetParent().GetNode<InteractionBox>("StockScreenIntBox").OnInteraction += activateStockScreen;
		GetParent().GetNode<InteractionBox>("StockScreenIntBox").OnInteractedLeave += exitUi;
		GetParent().GetNode<InteractionBox>("Purchase Screen Box").OnInteraction += activatePurchaseScreen;
		GetParent().GetNode<InteractionBox>("Purchase Screen Box").OnInteractedLeave += exitUi;
		GetParent().GetNode<InteractionBox>("SellScreenBox").OnInteraction += activateSellScreen;
		GetParent().GetNode<InteractionBox>("SellScreenBox").OnInteractedLeave += exitUi;
		GetParent().GetNode<InteractionBox>("News Screen Box2").OnInteraction += activateNewsScreen;
		GetParent().GetNode<InteractionBox>("News Screen Box2").OnInteractedLeave += exitUi;
	}


	public void updateTime()
	{
		currentTime += 0.00347222222f * 0.008f * timeScale;
		stockManager.updateStocks(currentTime);
		enemyManager.updateSpawns(player, currentTime);

		if (currentTime >= 10.00f) {
			onGameCompletion();
		}
	}

	public void onGameCompletion () {
		EmitSignal(SignalName.GameCompletion, money, 0);
		exitUi();
		gameTick.Paused = true;
	}

	public void activateStockScreen()
	{
		ActiveUi = GetNode<CanvasLayer>("UIContainer/StocksInfoScreen");
		// enemyManager.spawnEnemy(enemyManager.getBestSpawnPoint(player.GlobalPosition));
	}

	public void activateNewsScreen()
	{
		ActiveUi = GetNode<CanvasLayer>("UIContainer/NewsScreen");
		// enemyManager.spawnEnemy(enemyManager.getBestSpawnPoint(player.GlobalPosition));
	}


	public void activatePurchaseScreen() {
		ActiveUi = GetNode<CanvasLayer>("UIContainer/PurchaseScreen");
		// enemyManager.spawnEnemy(enemyManager.getBestSpawnPoint(player.GlobalPosition));
	}


	public void activateSellScreen() {
		ActiveUi = GetNode<CanvasLayer>("UIContainer/SellScreen");
		// enemyManager.spawnEnemy(enemyManager.getBestSpawnPoint(player.GlobalPosition));
	}
	public void exitUi()
	{
		ActiveUi = GetNode<CanvasLayer>("UIContainer/PlayerHUD");
	}
	public static string timeToHour(double time)
	{

		string res = "";

		time = time % 1;

		time = time * 24.0;

		if (time >= 12.0)
		{
			res += " PM";
		}
		else
		{
			res += " AM";
		}

		time = (time % 12);
		if (time < 1)
		{
			time += 12;
		}
		double mins = time % 1;

		time = (int)time;

		time += mins * .60;

		res = string.Format("{0:N2}", time) + res;

		return res.Replace('.', ':');
	}

}
