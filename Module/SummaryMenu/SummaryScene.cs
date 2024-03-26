using Godot;
using System;

public partial class SummaryScene : Node2D
{
	public void Exit() {
		GetParent().GetParent<SceneManager>().transitionScenes(GD.Load<PackedScene>("res://Module/MainMenu/MainMenu.tscn"));
	}

	public void updateSummary(float money, int enemies) {
		GD.Print("UPAD");
		GetNode<Label>("CanvasLayer/MoneyEarned").Text = "$" + money.ToString("0.00");
		GetNode<Label>("CanvasLayer/Enemies Defeated").Text = enemies.ToString();
		GetNode<Label>("CanvasLayer/Firm Saved").Text = (money > 10000)? "The Firm Was \nSaved!": "You Lost The\nFirm.";
		GetNode<Label>("CanvasLayer/Firm Saved").LabelSettings.FontColor = (money > 10000)? new Color(0, 255, 0) : new Color(255, 0, 0);
	}
}
