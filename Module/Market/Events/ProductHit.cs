using Godot;
using System;

public partial class ProductHit : Event
{
    public override double eventFunction(double time)
    {
        base.eventFunction(time);
        time -= eventStartTime;
        return eventScale * 0.01 * Math.Pow(2, (10 * time + 2));
    }
}
