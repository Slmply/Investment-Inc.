using Godot;
using System;

public partial class EmployeeStrike : Event
{
    public override double eventFunction(double time)
    {
        base.eventFunction(time);
        return eventScale * evaluate(time);
    }

    private static double evaluate(double x)
    {
        double result =  -.5f * Math.Pow(x, .4f);
        return result;
    }
}
