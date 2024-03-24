using Godot;
using System;

public partial class EmployeeStrike : Event
{
    public override double eventFunction(double time)
    {
        base.eventFunction(time);
        time -= eventStartTime;
        return eventScale * -evaluate(time);
    }

    private static double evaluate(double x)
    {
        double result =  -.5f * Math.Pow(x, .4f) + variation(x);
        return result;
    }
}
