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

    public override void _Ready()
    {
        sceneTransitionPlayer = GetNode<AnimationPlayer>("SceneTransition/SceneTransitionPlayer");
		sceneHolder = GetNode<Node2D>("SceneHolder");
		transitionScenes(startingScene);
    }

	public void transitionScenes(PackedScene newScene) {
		sceneTransitionPlayer.Play("SceneTransitionIn");
		sceneToLoad = newScene;
		sceneTransitionPlayer.AnimationFinished += (name) => loadNextScene();
	}


	public void loadNextScene() {
		if (sceneToLoad != null) {
				foreach (Node n in sceneHolder.GetChildren() ) {
				n.QueueFree();
			}
			sceneHolder.AddChild(sceneToLoad.Instantiate());

			sceneTransitionPlayer.Play("SceneTransitionOut");
			sceneToLoad = null;
		}
	}

	public void dummyTransition(Func<float> func) {
		sceneTransitionPlayer.Play("DummyTransition");
		dummyFunc = func;
	}

	public void dummyTransitionCall() {
		dummyFunc();
	}
}
