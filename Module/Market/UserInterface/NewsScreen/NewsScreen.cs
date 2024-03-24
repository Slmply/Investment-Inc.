using Godot;
using System;

public partial class NewsScreen : CanvasLayer
{

	private PackedScene newsWidgetScreen = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		newsWidgetScreen = (PackedScene)GD.Load("res://Module/Market/UserInterface/NewsScreen/news_widget.tscn");
	}

	public void addNewsWidget(Stock eventStock, Event stockEvent, float time) {
		Control newsWidget = (Control) newsWidgetScreen.Instantiate();

		newsWidget.GetNode<Label>("ColorRect/HBoxContainer/CompanySymbol").Text = "";
		foreach(char ch in eventStock.stockSymbol) {
			newsWidget.GetNode<Label>("ColorRect/HBoxContainer/CompanySymbol").Text += "\n" + ch;
		}
		newsWidget.GetNode<Label>("ColorRect/HBoxContainer/CompanySymbol").Text = newsWidget.GetNode<Label>("ColorRect/HBoxContainer/CompanySymbol").Text.Substring(1);

		newsWidget.GetNode<Label>("ColorRect/HBoxContainer/VBoxContainer/EventName").Text = stockEvent.eventName;
		newsWidget.GetNode<Label>("ColorRect/HBoxContainer/VBoxContainer/Event Description").Text = eventStock.companyName + stockEvent.eventDescription;

		newsWidget.GetNode<Label>("ColorRect/HBoxContainer/VBoxContainer2/Projected Time").Text = GameManager.timeToHour(time) + "\n" + StockGraph.timeToDay(time);

		newsWidget.GetNode<CompanyLogo>("ColorRect/HBoxContainer/CompanyLogo").setStockLogo(eventStock.stockSymbol);

		GetNode<VBoxContainer>("ColorRect/VBoxContainer/ColorRect/ScrollContainer/NewsWidgetContainers").AddChild(newsWidget);
		GetNode<VBoxContainer>("ColorRect/VBoxContainer/ColorRect/ScrollContainer/NewsWidgetContainers").MoveChild(newsWidget, 0);
		updateNewsScreen();
	}

	public void updateNewsScreen() {

		if (GetNode<VBoxContainer>("ColorRect/VBoxContainer/ColorRect/ScrollContainer/NewsWidgetContainers").GetChildCount() > 0) {
			GetNode<Label>("ColorRect/NoNewsLabel").Visible = false;
		} else {
			GetNode<Label>("ColorRect/NoNewsLabel").Visible = false;
		}

	}
}
