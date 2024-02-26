using Godot;
using System;
using System.Diagnostics;

public partial class StockTester : Node2D
{

	[Export]
	public Stock tStock;
	[Export]
	public StockGraph graph;

	private float i = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		graph.targetStock = tStock;
		tStock.init();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void StockTimerTimout()
	{
		tStock.update(i);
		graph.updateStockGraph();
		GD.Print(tStock.stockPrice);
		if (i == 5) tStock.beginEvent(GD.Load<Event>("res://Module/Market/Events/TestingEvent.tres"), i);
		i += 0.1f;
	}


}
