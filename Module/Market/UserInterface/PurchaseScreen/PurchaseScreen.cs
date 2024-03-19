using Godot;
using System;

public partial class PurchaseScreen : CanvasLayer
{
	private VBoxContainer stockInfoContainer;
	private PackedScene stockInfoWidget;
	private stock_info activeStockInfo;
	private Stock currentStock;
	private float currentBuyAmt;
	[Export]
	public GameManager gm;

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

		currentBuyAmt = 0;

		currentStock = activeStockInfo.stock;
		updatePurchaseScreen();
	}

	public void updatePurchaseScreen() {

		if (currentStock != null) {
			GetNode<Label>("HBoxContainer/PurchaseContainer/ColorRect/GridContainer/PurchaseLabel").Text = "Purchase: " + currentStock.companyName;
		} else {
			GetNode<Label>("HBoxContainer/PurchaseContainer/ColorRect/GridContainer/PurchaseLabel").Text = "N/A";
		}

		GetNode<Label>("HBoxContainer/PurchaseContainer/ColorRect/GridContainer/ColorRect/BuyAmtLabel").Text = currentBuyAmt.ToString("0.00");

	}

	public void buyAmtUp() {
		currentBuyAmt++;
		clampBuyAmount();
		updatePurchaseScreen();
	}

	public void buyAmtDown() {
		currentBuyAmt--;
		clampBuyAmount();
		updatePurchaseScreen();
	}

	public void clampBuyAmount() {
		if (currentStock != null) {
			if (currentBuyAmt * (float)currentStock.stockPrice >= gm.money) {
				currentBuyAmt = gm.money / (float)currentStock.stockPrice;
			}
			if (currentBuyAmt < 0 ) {
				currentBuyAmt = 0;
			}
		} else {
			currentBuyAmt = 0;
		}
	}

	public void OnPurchasePressed() {

		if (gm.money >= currentBuyAmt * currentStock.stockPrice) {
			gm.GetNode<StockManager>("Stock Manager").purchaseStock(currentStock, currentBuyAmt);
		} else {

		}

		
	}
}