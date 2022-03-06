using UnityEngine;
using ZestGames.Vibrate;

public class Heli : ObstacleBase
{
    private Player player;
    public Player Player => player == null ? player = FindObjectOfType<Player>() : player;

    [SerializeField, Tooltip("Do you want to push the hitter back?")] private bool pushBack = false;
    [SerializeField, Tooltip("Force that will be applied when something hit this.")] private float pushBackForce = 200f;

    public override void Execute()
    {
        // apply force to the object that hit this object.
        AudioHandler.PlayAudio(AudioHandler.AudioType.Hit_Rock);
        if (Vibration.HasVibrator())
            Vibration.VibratePredefined(0, true);

        if (pushBack)
            PushBack(Player.rb);

        base.Execute();
    }

    private void PushBack(Rigidbody rb) => rb.AddForce(Vector3.back * pushBackForce, ForceMode.Impulse);
}