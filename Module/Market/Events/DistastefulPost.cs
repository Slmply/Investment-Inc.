using Godot;
using System;

public partial class DistastefulPost : Event
{
    public override double eventFunction(double time)
    {
        base.eventFunction(time);
        return eventScale * evaluate(time);
    }

    private static double evaluate(double x)
    {
        double result = -Math.Pow(2, 1.2 * x) + 1;
        return result;
    }

}
