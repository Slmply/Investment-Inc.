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

	private StockManager stockManager;
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

	public override void _Ready()
	{
		stockManager = GetNode<StockManager>("Stock Manager");
		stockManager.updateStocks(0.0f);
		gameTick = GetNode<Timer>("GameTick");
		gameTick.Start();
		GetParent().GetNode<InteractionBox>("StockScreenIntBox").OnInteraction += activateStockScreen;
		GetParent().GetNode<InteractionBox>("StockScreenIntBox").OnInteractedLeave += exitUi;
	}


	public void updateTime()
	{
		currentTime += 0.00347222222f * timeScale;
		stockManager.updateStocks(currentTime);
	}

	public void activateStockScreen()
	{
		ActiveUi = GetNode<CanvasLayer>("UIContainer/StocksInfoScreen");
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
