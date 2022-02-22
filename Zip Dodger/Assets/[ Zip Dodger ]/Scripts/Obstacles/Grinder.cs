using UnityEngine;
using ZestGames.Vibrate;

public class Grinder : ObstacleBase
{
    private Player player;
    public Player Player { get { return player == null ? player = FindObjectOfType<Player>() : player; } }

    [SerializeField, Tooltip("Force that will be applied when something hit this.")] private float pushBackForce = 100f;

    public override void Execute()
    {
        // apply force to the object that hit this object.
        AudioHandler.PlayAudio(AudioHandler.AudioType.Pickup_Trap);
        if (Vibration.HasVibrator())
            Vibration.VibratePredefined(0, true);

        PushBack(Player.rb);

        base.Execute();
    }

    private void PushBack(Rigidbody rb) => rb.AddForce(Vector3.back * pushBackForce, ForceMode.Impulse);
}
