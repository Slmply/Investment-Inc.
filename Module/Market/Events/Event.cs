using Godot;
using System;
using System.Drawing;

[GlobalClass]
public abstract partial class Event : Resource
{

    [Export]
    public string eventName;
    [Export]
    public string eventDescription;
    [Export]
    public float eventDuration;
    [Export]
    public double eventScale;
    protected double eventStartTime;

    [Signal]
    public delegate void EventCompletionEventHandler(double time);

    public void beginEvent(double time)
    {
        eventStartTime = time;
        GD.Print("Event Started: " + eventName);
    }
    public virtual double eventFunction(double time)
    {
        if (time >= eventDuration)
        {
            EmitSignal(SignalName.EventCompletion, time);
            return -1;
        }
        return 1;
    }

    public static double variation(double x) {
        return .07 * Math.Sin(3 * x - 2) + 0.04 * Math.Cos(3 * x) - 0.008 * Math.Sin(576 * x)
                        + 0.08 * Math.Cos(32 * x) - 0.08 * Math.Cos(2 * x) - 0.05 * Math.Sin(10 * x) + 0.03 * Math.Cos(80 * x + 3);
    }

}
