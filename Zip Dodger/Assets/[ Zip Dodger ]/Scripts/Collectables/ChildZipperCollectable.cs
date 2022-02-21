using UnityEngine;
using ZestGames.Vibrate;

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

        AudioHandler.PlayAudio(AudioHandler.AudioType.Pickup_Coin);
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
