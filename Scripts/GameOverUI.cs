using Godot;
using System;

public partial class GameOverUI : Control
{

    [Export]
    private AnimationPlayer TransitionAnimator;

    private bool TryAgain = false;


    public override void _Ready()
    {
        base._Ready();
        if (AudioManager.Instance.AmbientPlayer != null)
            AudioManager.Instance.AmbientPlayer.StopMusic(0.2f);

        TransitionAnimator.Play("SceneTransitionIn");
    }

    public void OnClickLoadMainMenu()
    {
        TransitionAnimator.Play("SceneTransitionOut");
        TryAgain = false;
    }

    public void OnClickTryAgain()
    { 
        TransitionAnimator.Play("SceneTransitionOut");
        TryAgain = true;
    }

    public void OnFinishLoadMainGameplayScene(StringName animationName)
    {
        if (animationName == "SceneTransitionOut")
        {
            if (TryAgain)
                SceneManager.Instance.LoadMainGameplayScene();
            else
                SceneManager.Instance.LoadMainMenuScene();
        }
    }
}
