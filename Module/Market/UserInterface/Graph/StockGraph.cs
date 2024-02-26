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

	private float minX, minY = 0;
	private float maxY = 50;
	private float maxX = 10;
	private float rectHeight, rectWidth;
	private float rectX, rectY;


	public override void _Process(double delta)
	{
		updateStockGraph();
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


		for (LinkedListNode<Godot.Vector2> i = targetStock.stockHistory.First; i != null; i = i.Next)
		{
			if (i.Value.X < minX)
			{
				minX = i.Value.X;
			}
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
			label.Text = string.Format("{0:N2}", maxY - (yTickSize * i));
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
			label.Text = string.Format("{0:N2}", (xTickSize * i));
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
	}
}
