using Godot;
using System;

public partial class PlayerHud : CanvasLayer
{
	[Export]
	public GameManager gameManager;

	private Label timeLabel;
	private Label moneyLabel;
	private Label dateLabel;

	public override void _Ready()
	{
		timeLabel = GetNode<Label>("TimeMoneyContainer/VBoxContainer/Time");
		moneyLabel = GetNode<Label>("TimeMoneyContainer/VBoxContainer/Money");
		dateLabel = GetNode<Label>("TimeMoneyContainer/VBoxContainer/Date");
	}


	public override void _Process(double delta)
	{
		timeLabel.Text = GameManager.timeToHour(gameManager.currentTime).ToLower();
		dateLabel.Text = StockGraph.timeToDay(gameManager.currentTime);
		moneyLabel.Text = "$" + gameManager.money.ToString("0.00");
		moneyLabel.Text += "\n($" + string.Format("{0:N2}", gameManager.stockManager.getTotalStockInventoryValue()) + ")";
	}

	public void playEventAnimation() {
		GetNode<AnimationPlayer>("AnimationPlayer").Play("NewsFlash");
	}
}
