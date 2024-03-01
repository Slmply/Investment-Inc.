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

    public override void _Ready()
	{
		stockManager = GetNode<StockManager>("Stock Manager");
		stockManager.updateStocks(0.0f);
		gameTick = GetNode<Timer>("GameTick");
		gameTick.Start();
		GetParent().GetNode<InteractionBox>("StockScreenIntBox").OnInteraction += activateStockScreen;
		GetParent().GetNode<InteractionBox>("StockScreenIntBox").OnInteractedLeave += disableStockScreen;
	}


	public void updateTime()
	{
		currentTime += 0.0033f * timeScale;
		stockManager.updateStocks(currentTime);
	}

	public void activateStockScreen() {
		stockManager.toggleStockInfo();
		Visible = false;
	}

	public void disableStockScreen() {
		stockManager.hideStockInfo();
		Visible = true;
	}

	

	public static string timeToHour(double time) {

		string res = "";

		time = time % 1;

		time = time * 24.0;

		if (time >= 12.0) {
			res += "PM";
		} else {
			res += "AM";
		}

		time =  (time % 12);
		if (time < 1) {
			time += 12;
		}
		double mins = time % 1;

		time = (int)time;

		time += mins * .60;

		res = string.Format("{0:N2}", time) + res;

		return res.Replace('.', ':');
	}

}
