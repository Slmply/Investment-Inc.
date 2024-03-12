using Godot;
using System;

public partial class stock_info : Control
{
	[Export]
	public Godot.Color deactiveColor;
	[Export]
	public Godot.Color activeColor;
	[Export]
	public Stock stock;
	private Boolean active;
	private Boolean mouseOver = false;

	private double lastUpdatePriceTime = 0;

	private const double STOCK_PRICE_UPDATE_INTERVAL = 500;

	[Signal]
	public delegate void activateScreenEventHandler(stock_info stockInfo, Stock stock);

	public Stock Stock
	{
		get
		{
			return stock;
		}
		set
		{
			stock = value;
			GetNode<Label>("StockName").Text = stock.companyName + "\n(" + stock.stockSymbol + ")";
		}
	}

	public Boolean Active
	{
		get
		{
			return active;
		}
		set
		{
			active = value;
			GetNode<ColorRect>("ColorRect").Color = (active) ? activeColor : deactiveColor;
			if (active)
			{
				EmitSignal(SignalName.activateScreen, this, stock);
				GD.Print("Activate Signal Emitted.");
			}
		}
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Select") && mouseOver)
		{
			Active = true;
		}
	}

	private double systemTimeMillis() {
		return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
	}

	public void updateStockPriceLabel()
	{
		if (systemTimeMillis() - lastUpdatePriceTime < STOCK_PRICE_UPDATE_INTERVAL) {
			return;
		}

		lastUpdatePriceTime = systemTimeMillis();

		Label l = GetNode<Label>("StockPrice");

		var last = stock.stockHistory.Last;
		var penult = last.Previous;

		if (!(last == null || penult == null))
		{
			if (stock.activeEvent != null)
			{
				l.Modulate = Godot.Color.FromHtml("ffff00");
			}
			else if (last.Value.Y - penult.Value.Y < 0)
			{
				l.Modulate = Godot.Color.FromHtml("ff0000");
			}
			else
			{
				l.Modulate = Godot.Color.FromHtml("00ff00");
			}
		}
		else
		{
			l.Modulate = Godot.Color.FromHtml("00ff00");
		}

		l.Text = "$" + string.Format("{0:N2}", stock.stockPrice);


	}

	public void OnMouseEnter()
	{
		mouseOver = true;
	}

	public void OnMouseExit()
	{
		mouseOver = false;
	}
}
