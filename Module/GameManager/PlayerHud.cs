using Godot;
using System;

public partial class PlayerHud : CanvasLayer
{
	[Export]
	public GameManager gameManager;

	private Label timeLabel;
	private Label moneyLabel;

	public override void _Ready()
	{
		timeLabel = GetNode<Label>("TimeMoneyContainer/VBoxContainer/Time");
		moneyLabel = GetNode<Label>("TimeMoneyContainer/VBoxContainer/Money");
	}


	public override void _Process(double delta)
	{
		timeLabel.Text = GameManager.timeToHour(gameManager.currentTime).ToLower();
		moneyLabel.Text = "$" + gameManager.money.ToString("0.00");
		moneyLabel.Text += " ($" + string.Format("{0:N2}", gameManager.stockManager.getTotalStockInventoryValue()) + ")";
	}
}
