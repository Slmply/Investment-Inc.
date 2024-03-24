using Godot;
using System;

public partial class GovernmentGrant : Event
{
    public override double eventFunction(double time)
    {
        base.eventFunction(time);
        return eventScale * evaluate(time);
    }

    private static double evaluate(double x)
    {
        double result = .5d * Math.Pow(x, .4d);
        return result;
    }
}
