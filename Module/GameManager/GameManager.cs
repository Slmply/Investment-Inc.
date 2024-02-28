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
	}


	public void updateTime()
	{
		currentTime += 0.0033f * timeScale;
		stockManager.updateStocks(currentTime);
	}



}
