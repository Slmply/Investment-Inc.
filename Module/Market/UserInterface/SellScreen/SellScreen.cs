using Godot;
using System;

public partial class SellScreen : CanvasLayer
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
			GetNode<Label>("HBoxContainer/PurchaseContainer/ColorRect/GridContainer/PurchaseLabel").Text = currentStock.companyName;
			GetNode<Label>("HBoxContainer/PurchaseContainer/ColorRect/GridContainer/CurrentShares Label").Text = "Shares Held: " + currentStock.sharesHeld.ToString("0.00");
		} else {
			GetNode<Label>("HBoxContainer/PurchaseContainer/ColorRect/GridContainer/PurchaseLabel").Text = "N/A";
			GetNode<Label>("HBoxContainer/PurchaseContainer/ColorRect/GridContainer/CurrentShares Label").Text = "";
		}

		GetNode<Label>("HBoxContainer/PurchaseContainer/ColorRect/GridContainer/Placeholder11/VBoxContainer/ColorRect/BuyAmtLabel").Text = currentBuyAmt.ToString("0.00");
		GetNode<Label>("HBoxContainer/PurchaseContainer/ColorRect/GridContainer/Placeholder11/VBoxContainer/SellAmount").Text = "$" + (currentBuyAmt * (float)currentStock.stockPrice).ToString("0.00");

	}

	private float getCurrentJump() {
		float res = currentStock.sharesHeld / 10f;
		return res;
	}

	public void buyAmtUp() {
		currentBuyAmt += getCurrentJump();
		clampBuyAmount();
		updatePurchaseScreen();
	}

	public void buyAmtDown() {
		currentBuyAmt -= getCurrentJump();
		clampBuyAmount();
		updatePurchaseScreen();
	}

	public void clampBuyAmount() {
		if (currentStock != null) {
			if (currentBuyAmt > currentStock.sharesHeld) {
				currentBuyAmt = currentStock.sharesHeld;
			} else if (currentBuyAmt < 0) {
				currentBuyAmt = 0;
			}
		} else {
			currentBuyAmt = 0;
		}
	}

	public void OnPurchasePressed() {

		clampBuyAmount();
		if (currentStock.sharesHeld >= currentBuyAmt) {
			gm.money += gm.GetNode<StockManager>("Stock Manager").sellStock(currentStock, currentBuyAmt);
			currentBuyAmt = 0;
			updatePurchaseScreen();
		} else {
			
		}

		
	}
}