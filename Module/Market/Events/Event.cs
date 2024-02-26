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
        if (time - eventStartTime >= eventDuration)
        {
            EmitSignal(SignalName.EventCompletion, time);
            return -1;
        }
        return 1;
    }

}
