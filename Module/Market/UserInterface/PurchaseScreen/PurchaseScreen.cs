using Godot;
using System;

public partial class PurchaseScreen : CanvasLayer
{
	private VBoxContainer stockInfoContainer;
	private PackedScene stockInfoWidget;
	private stock_info activeStockInfo;

	public override void _Ready()
	{
		
	}

	public override void _Process(double delta)
	{
		if (Visible) {
			foreach (stock_info si in stockInfoContainer.GetChildren()){
				si.updateStockPriceLabel();
			}
		}
		
	}

	public void loadStocks(Stock[] stocks)
	{

		stockInfoContainer = GetNode<VBoxContainer>("HBoxContainer/ScrollContainer/StockInfoContainer");

		foreach (Stock s in stocks)
		{
			stockInfoWidget = GD.Load<PackedScene>("res://Module/Market/UserInterface/InfoScreen/stock_info.tscn");
			stock_info si = (stock_info)stockInfoWidget.Instantiate();
			GD.Print(si);
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
	}


}