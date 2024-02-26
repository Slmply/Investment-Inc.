using Godot;
using System;
using System.Collections.Generic;
using System.Numerics;

public partial class Stock : Resource
{
    [Export]
    public string companyName;
    [Export]
    public string stockSymbol;
    [Export]
    public double stockPrice;
    [Export]
    public double volatility;
    [Export]
    public double expectedGrowth;
    [Export]
    public Event activeEvent = null;
    public LinkedList<Godot.Vector2> stockHistory;
    private double startingPrice;
    private double timeOffset;
    private double seed;

    public void init()
    {
        this.startingPrice = stockPrice;
        this.timeOffset = 0;
        Random rnd = new Random();
        this.seed = rnd.NextDouble() * 100;
        GD.Print("Initializing - " + companyName);
    }

    public void beginEvent(Event newEvent, double time)
    {
        this.activeEvent = newEvent;
        activeEvent.beginEvent(time);
        activeEvent.EventCompletion += endEvent;
        this.startingPrice = stockPrice;
        GD.Print(companyName + " " + activeEvent.eventDescription);
    }

    public void endEvent(double time)
    {
        timeOffset = time;
        GD.Print("Event " + activeEvent.eventName + " Ended");
        activeEvent.EventCompletion -= endEvent;
        activeEvent = null;
        this.startingPrice = stockPrice;
    }

    public double update(double time)
    {

        double res = stockPrice;
        if (activeEvent != null)
        {
            res = activeEvent.eventFunction(time);

            if (activeEvent != null)
            {
                res = (res + 1) * startingPrice;
                stockHistory.AddLast(new Godot.Vector2((float)time, (float)res));
            }

        }
        else
        {
            var offsetTime = time - timeOffset;
            res = this.stockPriceEq(offsetTime);

            res = (res + 1) * startingPrice;
            stockHistory.AddLast(new Godot.Vector2((float)time, (float)res));
        }


        stockPrice = res;
        return res;
    }

    private double stockPriceEq(double time)
    {
        var i = 0.1d * Math.Sin((10 * volatility) * (time + volatility * seed)) + (expectedGrowth * time) / 15;
        i += -0.2d * volatility * Math.Sin(time - 3);
        i += 0.06d * volatility * Math.Sin(time);
        i -= 0.8d * volatility * volatility * Math.Cos(10 * (time + seed + 4.8));
        i += 0.8d * volatility * volatility * Math.Cos(20 * (time + seed + 54.83425));
        i -= 0.8d * volatility * volatility * Math.Cos(8 * (time + seed + 8.8));
        i -= volatility * volatility * volatility * Math.Cos(70 * (time + seed + 5.8));
        i -= 0.4d * volatility * volatility * Math.Cos(100 * (time + seed + 43.8342));

        return i;
    }

    public Stock()
    {
        this.companyName = "companyName";
        this.stockSymbol = "stockSymbol";
        this.stockPrice = 0;
        this.volatility = 0;
        this.expectedGrowth = 0;
        this.stockHistory = new LinkedList<Godot.Vector2>();
        this.startingPrice = stockPrice;
        Random rnd = new Random();
        this.seed = rnd.NextDouble() * 100;
    }

    public Stock(string companyName, string stockSymbol, double stockPrice, double volatility, double expectedGrowth)
    {
        this.companyName = companyName;
        this.stockSymbol = stockSymbol;
        this.stockPrice = stockPrice;
        this.volatility = volatility;
        this.expectedGrowth = expectedGrowth;
        this.stockHistory = new LinkedList<Godot.Vector2>();
        this.startingPrice = stockPrice;
        Random rnd = new Random();
        this.seed = rnd.NextDouble() * 100;
    }


}
