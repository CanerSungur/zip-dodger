using UnityEngine;
using UnityEngine.UI;
using System;
using ZestGames.Utility;

/// <summary>
/// Plays sound and animation first, then executes the action given.
/// Give the action as a parameter of CustomButton's event.
/// </summary>
public class CustomButton : Button
{
    private Animation anim;
    private float animationDuration = 0.5f;
    public event Action<Action> OnClicked;

    protected override void OnEnable()
    {
        anim = GetComponent<Animation>();

        OnClicked += Clicked;
    }

    protected override void OnDisable()
    {
        OnClicked -= Clicked;
    }

    private void Clicked(Action action)
    {
        // Play audio
        AudioHandler.PlayAudio(AudioHandler.AudioType.Button_Click);

        // Reset and Play the animation
        anim.Rewind();
        anim.Play();

        // Do the action with delay
        Utils.DoActionAfterDelay(this, animationDuration, () => action());
    }

    public void ClickTrigger(Action action) => OnClicked?.Invoke(action);
}
