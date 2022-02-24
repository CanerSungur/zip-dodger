using UnityEngine;
using ZestGames.Vibrate;
using ZestGames.Utility;

public class ChildZipperCollectable : CollectableBase
{
    [Header("-- REFERENCES --")]
    [SerializeField] private ChildZipperCollectableMovement movement;
    private Collider coll;

    [Header("-- PROOERTIES --")]
    [SerializeField, Tooltip("Value of this zipper. How many zipper pairs will be added upon pick up?")] private int value = 1;
    [SerializeField, Tooltip("Should start moving when enabled?")] private bool startMovingOnEnable;

    [Header("-- MOVEMENT SETUP --")]
    [SerializeField, Tooltip("Movement animations will last this much.")] private float cycleLength = 2f;

    public float CycleLength => cycleLength;

    public override void Apply()
    {
        coll.enabled = false;

        AudioHandler.PlayAudio(AudioHandler.AudioType.Pickup_Zipper);
        if (Utils.RollDice(20)) AudioHandler.PlayAudio(AudioHandler.AudioType.Game_Win, 0.7f, 1f);

        if (Vibration.HasVibrator())
            Vibration.VibratePredefined(0, true);

        // Apply instantly.
    }

    private void OnEnable()
    {
        coll = GetComponent<Collider>();
        coll.enabled = true;

        if (startMovingOnEnable) movement.StartMovingTrigger();
    }
}
