using Godot;
using System;

public partial class TestingEvent : Event
{
	public override double eventFunction (double time) {
        base.eventFunction(time);
		time -= eventStartTime;
        return (0.1 * eventScale * time);
    }
}
