using Godot;
using System;

public partial class StocksInfoScreen : Control
{
	private VBoxContainer stockInfoContainer;
	private PackedScene stockScreen;
	private PackedScene stockInfoWidget;
	private stock_info activeStockInfo;
	private StockGraph activeStockGraph;

	public override void _Ready()
	{
		stockScreen = GD.Load<PackedScene>("res://Module/Market/UserInterface/Graph/stock_graph.tscn");
		stockInfoWidget = GD.Load<PackedScene>("res://Module/Market/UserInterface/stock_info.tscn");
	}

	public override void _Process(double delta)
	{
		foreach (stock_info si in stockInfoContainer.GetChildren())
		{
			si.updateStockPriceLabel();
		}
	}

	public void loadStocks(Stock[] stocks)
	{

		stockInfoContainer = GetNode<VBoxContainer>("HBoxContainer/ScrollContainer/StockInfoContainer");

		foreach (Stock s in stocks)
		{
			stock_info si = (stock_info)stockInfoWidget.Instantiate();
			si.Stock = s;
			stockInfoContainer.AddChild(si);
			si.activateScreen += stockSelected;
		}
	}





	public void stockSelected(stock_info stockInfo, Stock stock)
	{

		GD.Print("Activate Signal Recieved " + stock.companyName);
		activeStockInfo = stockInfo;

		foreach (stock_info si in stockInfoContainer.GetChildren())
		{
			if (si != stockInfo)
			{
				si.Active = false;
			}
		}

		Control sgContainer = GetNode<Control>("HBoxContainer/StockGraphContainer");

		foreach (Node n in sgContainer.GetChildren())
		{
			n.QueueFree();
		}

		StockGraph sg = (StockGraph)stockScreen.Instantiate();
		sg.targetStock = stock;

		sgContainer.AddChild(sg);
		activeStockGraph = sg;
	}


}
