using Godot;
using Godot.Collections;
using System;
using System.Collections;

public partial class StockManager : Node
{
	[Export]
	public Stock[] stocks;
	[Export]
	public Event[] eventList;
	[Export]
	public float minEventWait;
	[Export]
	public float maxEventWait;
	[Export]
	public float eventNotifyPercentage = 0.6f;

	private float nextEventTime;
	private float i = 0;
	[Export]
	public StocksInfoScreen sis = null;
	[Export]
	public PurchaseScreen pss = null;
	[Export]
	public SellScreen ss = null;
	[Export]
	public NewsScreen ns = null;
	[Export]
	public PlayerHud ph = null;

	public Event nextEvent;
	public Stock nextEventStock;
	public float nextEventNotifyTime = 0.0f;

	public override void _Ready()
	{
		// sis = GetNode<StocksInfoScreen>("StocksInfoScreen");
		initializeStocks();
	}

	public void initializeStocks()
	{
		Random random = new Random();
		nextEventTime = ((float)random.NextDouble() * maxEventWait) + minEventWait;
		nextEventNotifyTime = nextEventTime * eventNotifyPercentage;
		eventSelection(0.0f);
		for (int k = 0; k < stocks.Length; k++)
		{
			stocks[k].init();
		}
		sis.loadStocks(stocks);
		pss.loadStocks(stocks);
		ss.loadStocks(stocks);
	}

	public void purchaseStock(Stock s, float amount)
	{
		s.sharesHeld += amount;
	}

	public float sellStock(Stock s, float amount) {

		s.sharesHeld -= amount;
		return (float)s.stockPrice * amount;
	}

	public void updateStocks(float time)
	{

		for (int k = 0; k < stocks.Length; k++)
		{
			stocks[k].update(time);
		}

		if (time >= nextEventTime)
		{
			nextEventStock.beginEvent(nextEvent, time);
			
			eventSelection(time);
		}
		if (time >= nextEventNotifyTime && nextEventNotifyTime >= 0) {
			ns.addNewsWidget(nextEventStock, nextEvent, nextEventTime);
			ph.playEventAnimation();
			nextEventNotifyTime = -1;
		}
	}

	public float getTotalStockInventoryValue() {
		float res = 0;

		foreach (Stock s in stocks) {
			res += (float) s.stockPrice * s.sharesHeld;
		}

		return res;
	}

	public void eventSelection(float time)
	{
		Random random = new Random();

		if (nextEvent != null) {
			nextEventTime = (float)random.NextDouble() * maxEventWait + minEventWait + time + nextEvent.eventDuration;
		} else {
			nextEventTime = (float)random.NextDouble() * maxEventWait + minEventWait + time ;
		}
		
		nextEventNotifyTime = time + ((nextEventTime - time) * eventNotifyPercentage);

		nextEventStock = randomWeighted();
		nextEvent = eventList[random.Next(0, eventList.Length)];

		

		
		GD.Print("Next Event At " + GameManager.timeToHour(nextEventTime));
		GD.Print("Next Notify Time At " + GameManager.timeToHour(nextEventNotifyTime));
	}

	public Stock randomWeighted()
	{

		float total = 0;

		foreach (Stock s in stocks)
		{
			total += (float)s.volatility;
		}

		Random rand = new Random();
		float random = (float)rand.NextDouble() * total;

		float cursor = 0;
		foreach (Stock s in stocks)
		{
			cursor += (float)s.volatility;
			if (cursor >= random)
			{
				return s;
			}
		}
		return null;
	}

}

