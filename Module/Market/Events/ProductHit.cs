using Godot;
using System;

public partial class ProductHit : Event
{
    public override double eventFunction(double time)
    {
        base.eventFunction(time);
        time -= eventStartTime;
        return eventScale * -evaluate(time);
    }

    private static double evaluate(double x)
    {
        double result = -Math.Sin(x) + 0.7 * Math.Sin(3 * x - 2) + 0.7 * Math.Cos(3 * x) - 0.008 * Math.Sin(576 * x)
                        + 0.08 * Math.Cos(32 * x) - 0.08 * Math.Cos(2 * x) - 0.05 * Math.Sin(10 * x) + 0.03 * Math.Cos(80 * x + 3);
        return result;
    }
}
