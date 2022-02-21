using UnityEngine;
using ZestGames.Vibrate;

public class Trap : CollectableBase
{
    [Header("-- REFERENCES --")]
    [SerializeField] private TrapMovement trapMovement;
    private Collider coll;

    [Header("-- PROPERTIES --")]
    [SerializeField, Tooltip("Value of this trap. Use this to effect whatever you want.")] private int value = 1;
    [SerializeField, Tooltip("Should start moving when enabled?")] private bool startMovingOnEnable;

    [Header("-- MOVEMENT SETUP --")]
    [SerializeField, Tooltip("Movement animations will last this much.")] private float cycleLength = 2f;

    public float CycleLength => cycleLength;

    public override void Apply()
    {
        coll.enabled = false;

        AudioHandler.PlayAudio(AudioHandler.AudioType.Pickup_Trap);
        if (Vibration.HasVibrator())
            Vibration.VibratePredefined(0, true);

        if (CollectStyle == CollectStyle.OnSite)
        {
            // Apply instantly.
            //CoinManager.GameManager.IncreaseCoinTrigger(Value);
        }
        else if (CollectStyle == CollectStyle.MoveToUI)
        {
            // Almost never will be used.
        }
    }

    private void OnEnable()
    {
        coll = GetComponent<Collider>();
        coll.enabled = true;

        if (startMovingOnEnable) trapMovement.StartMovingTrigger();
    }
}
