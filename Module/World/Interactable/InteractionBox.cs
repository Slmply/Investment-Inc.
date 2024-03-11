using Godot;
using System;

public partial class InteractionBox : Area2D
{
	[Export]
	public string interactionPrompt;
	[Export]
	public Shape2D collisionShape;
	[Signal]
	public delegate void OnInteractionEventHandler();
	[Signal]
	public delegate void OnInteractedLeaveEventHandler();

	private bool playerInside = false;
	private bool interacted = false;

	public override void _Ready() {
		GetNode<Label>("InteractionPrompt/Label").Text = interactionPrompt;
	}

	public void OnBodyEntered(Node2D body) {
		if (body is Player) {
			playerInside = true;
			interacted = false;
			GetNode<Control>("InteractionPrompt").Visible = true;
		}
	}

	public void OnBodyExited(Node2D body) {
		if (body is Player) {
			playerInside = false;
			GetNode<Control>("InteractionPrompt").Visible = false;

			if (interacted) {
				EmitSignal(SignalName.OnInteractedLeave);
			}

			interacted = false;

		}
	}

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Interact") && playerInside) {
			if (interacted) {
				EmitSignal(SignalName.OnInteractedLeave);
				interacted = false;
			} else {
				EmitSignal(SignalName.OnInteraction);
				interacted = true;
			}
			
		}
    }


}
