using Godot;
using System;
using System.Collections.Generic;
using System.IO.Pipes;

public partial class StockGraph : Control
{
	[Export]
	public Godot.Color growthColor;
	[Export]
	public Godot.Color decayColor;
	[Export]
	public Godot.Color eventColor;
	[Export]
	public float lineWidth;
	[Export]
	public Godot.Color backgroundColor;
	[Export]
	public Stock targetStock = null;
	[Export]
	public Line2D line;
	[Export]
	public int xTicks = 7;
	[Export]
	public int yTicks = 7;
	[Export]
	public string xLabel, yLabel;
	[Export]
	public LabelSettings labelSettings;
	[Export]
	public Range rng = Range.AllTime;

	private float minX, minY = 0;
	private float maxY = 0;
	private float maxX = 1;
	private float rectHeight, rectWidth;
	private float rectX, rectY;
	public GameManager gm = null;
	private float startTS = 1.3f;


	public override void _Process(double delta)
	{
		updateStockGraph();

		if (GetNode<Button>("GridContainer/Button").ButtonPressed == true) {
			gm.timeScale = startTS * 10;
			gm.getMusicPlayer().PitchScale = 1.75f;
		} else {
			gm.timeScale = startTS;
			gm.getMusicPlayer().PitchScale = 1f;
		}
	}

	public enum Range{
		Today,
		TwentyFour,
		FiveDay,
		AllTime

	}

	public static string timeToDay(double time) {

		int day = (int)time;

		return "6/" + (day + 1) + "/2024";
	}

	public string timeToHour(double time) {

		string res = "";

		time = time % 1;

		time = time * 24.0;

		if (time >= 12.0) {
			res += "PM";
		} else {
			res += "AM";
		}

		time = time % 12;
		if (time < 1) {
			time += 12;
		}
		double mins = time % 1;

		time = (int)time;

		time += mins * .60;

		res = string.Format("{0:N2}", time) + res;

		return res.Replace('.', ':');
	}

	public void updateStockGraph()
	{

		var colorRect = GetNode<ColorRect>("GridContainer/ColorRect");

		GetNode<Label>("GridContainer/YLabel").Text = yLabel;
		GetNode<Label>("GridContainer/XLabel").Text = xLabel;

		GetNode<ColorRect>("GridContainer/ColorRect").Color = backgroundColor;

		if (targetStock == null) return;
		line.Width = lineWidth;

		var last = targetStock.stockHistory.Last;
		var penult = last.Previous;

		if (!(last == null || penult == null))
		{
			if (targetStock.activeEvent != null)
			{
				line.DefaultColor = eventColor;
			}
			else if (last.Value.Y - penult.Value.Y < 0)
			{
				line.DefaultColor = decayColor;
			}
			else
			{
				line.DefaultColor = growthColor;
			}
		}
		else
		{
			line.DefaultColor = growthColor;
		}

		// Current mode
		switch(rng) 
		{
			case Range.Today:
				minX = (int)targetStock.stockHistory.Last.Value.X;
				line.Width = 2;
				xTicks = 7;
				break;
			case Range.TwentyFour:
				minX = Math.Clamp(targetStock.stockHistory.Last.Value.X - 1, 0, int.MaxValue);
				line.Width = 2;
				xTicks = 7;
				break;
			case Range.FiveDay:
				minX = Math.Clamp(targetStock.stockHistory.Last.Value.X - 5, 0, int.MaxValue);
				line.Width = 2;
				xTicks = 6;
				break;
			case Range.AllTime:
				minX = 0;
				line.Width = 2;
				xTicks = 7;
				break;
		}
		

		for (LinkedListNode<Godot.Vector2> i = targetStock.stockHistory.First; i != null; i = i.Next)
		{
			if (i.Value.X > maxX)
			{
				maxX = i.Value.X;
			}
			if (i.Value.Y > maxY)
			{
				maxY = i.Value.Y;
			}
		}

		VBoxContainer yTickCont = GetNode<VBoxContainer>("GridContainer/YLabelContainer");

		foreach (Node k in yTickCont.GetChildren())
		{
			yTickCont.RemoveChild(k);
			k.QueueFree();
		}

		float yTickSize = (maxY - minY) / (yTicks - 1);
		for (int i = 0; i < yTicks; i++)
		{
			Label label = new Label();
			label.SizeFlagsVertical = SizeFlags.ExpandFill;
			label.HorizontalAlignment = HorizontalAlignment.Center;
			label.VerticalAlignment = VerticalAlignment.Center;
			label.Text = "$" + string.Format("{0:N2}", maxY - (yTickSize * i));
			label.LabelSettings = labelSettings;
			yTickCont.AddChild(label);
		}

		HBoxContainer xTickCont = GetNode<HBoxContainer>("GridContainer/XLabelContainer");

		foreach (Node k in xTickCont.GetChildren())
		{
			xTickCont.RemoveChild(k);
			k.QueueFree();
		}

		float xTickSize = (maxX - minX) / (xTicks - 1);
		for (int i = 0; i < xTicks; i++)
		{
			Label label = new Label();
			label.SizeFlagsHorizontal = SizeFlags.ExpandFill;
			label.VerticalAlignment = VerticalAlignment.Center;
			label.HorizontalAlignment = HorizontalAlignment.Center;
			label.Text = timeToHour(xTickSize * i) + '\n' + timeToDay(xTickSize * i);
			label.LabelSettings = labelSettings;
			xTickCont.AddChild(label);
		}



		line.ClearPoints();

		rectWidth = colorRect.Size.X;
		rectHeight = colorRect.Size.Y;

		rectX = rectWidth / xTicks;
		rectY = rectHeight / yTicks;

		rectWidth = rectX * (xTicks - 1);
		rectHeight = rectY * (yTicks - 1);

		for (LinkedListNode<Godot.Vector2> i = targetStock.stockHistory.First; i != null; i = i.Next)
		{
			if (i.Value.X < minX) continue;
			line.AddPoint(new Godot.Vector2(scaleX(i.Value.X), scaleY(i.Value.Y)));
		}

	}

	private float scaleX(float pos)
	{
		float dx = maxX - minX;
		return ((pos - minX) * rectWidth / dx) + rectX / 2;
	}

	private float scaleY(float pos)
	{
		float dy = maxY - minY;
		return rectHeight - ((pos - minY) * rectHeight / dy) + rectY / 2;
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		updateStockGraph();
		if (gm != null) {
			startTS = gm.timeScale; 
		}
	}

	public void OnTodayPressed() 
	{
		rng = Range.Today;
	}
	public void OnTwentyFourPressed() 
	{
		rng = Range.TwentyFour;
	}
	public void OnFiveDaysPressed() 
	{
		rng = Range.FiveDay;
	}
	public void OnAllTimePressed() 
	{
		rng = Range.AllTime;
	}
}
