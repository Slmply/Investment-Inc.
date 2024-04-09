using Godot;
using System;

public partial class SceneManager : Node2D
{

	private AnimationPlayer sceneTransitionPlayer;
	private PackedScene sceneToLoad;
	private Node2D sceneHolder;
	private Func<float> dummyFunc;
	[Export]
	public PackedScene startingScene;

	private float savedMoney;
	private int savedEnemies;

	private bool nextSummary = false;

	public override void _Ready()
	{
		sceneTransitionPlayer = GetNode<AnimationPlayer>("SceneTransition/SceneTransitionPlayer");
		sceneHolder = GetNode<Node2D>("SceneHolder");
		transitionScenes(startingScene);
	}

	public void summary(float money, int enemies) {
		nextSummary = true;
		savedMoney = money;
		savedEnemies = enemies;
	}

	public void transitionScenes(PackedScene newScene) {
		sceneTransitionPlayer.Play("SceneTransitionIn");
		sceneToLoad = newScene;
		sceneTransitionPlayer.AnimationFinished += (name) => loadNextScene();
	}


	public void loadNextScene() {
		if (sceneToLoad != null) {
			foreach (Node nod in sceneHolder.GetChildren() ) {
				nod.QueueFree();
			}
			Node n = sceneToLoad.Instantiate();
			sceneHolder.AddChild(n);

			sceneTransitionPlayer.Play("SceneTransitionOut");
			sceneToLoad = null;

			if (nextSummary) {
				((SummaryScene) n).updateSummary(savedMoney, savedEnemies);
			}

			
		}
		nextSummary = false;
	}

	public void dummyTransition(Func<float> func) {
		sceneTransitionPlayer.Play("DummyTransition");
		dummyFunc = func;
	}

	public void dummyTransitionCall() {
		dummyFunc();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Exit")) {
			dummyTransition(() => {GetTree().Quit(); return 0;});
		}
	}
}
