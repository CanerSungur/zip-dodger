using UnityEngine;

public class Coin : CollectableBase
{
    private CollectableManager collectableManager;
    public CollectableManager CollectableManager { get { return collectableManager == null ? collectableManager = FindObjectOfType<CollectableManager>() : collectableManager; } }
    
    [Header("-- REFERENCES --")]
    [SerializeField] private CoinMovement coinMovement;
    private Collider coll;

    [Header("-- PROPERTIES --")]
    [SerializeField, Tooltip("Value of this coin. This will be added to Player Coin Amount.")] private int value = 1;

    [Header("-- MOVEMENT SETUP --")]
    [SerializeField, Tooltip("Speed of this coin moving to HUD Coin Position.")] private float movementSpeed = 2f;

    // Properties
    public float MovementSpeed => movementSpeed;
    public int Value => value;

    public override void Apply()
    {
        if (coll) coll.enabled = false;

        AudioHandler.PlayAudio(AudioHandler.AudioType.Pickup_Coin);

        if (CollectStyle == CollectStyle.OnSite)
        {
            // Apply instantly.
            CollectableManager.GameManager.IncreaseCoinTrigger(Value);
        }
        else if (CollectStyle == CollectStyle.MoveToUI)
        {
            // Activate coin movement to UI.
            coinMovement.StartMovingTrigger();
        }
    }

    private void OnEnable()
    {
        if (TryGetComponent(out coll))
            coll.enabled = true;
    }
}
