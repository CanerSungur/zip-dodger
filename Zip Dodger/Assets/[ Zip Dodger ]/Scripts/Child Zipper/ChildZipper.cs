using UnityEngine;
using System;

public class ChildZipper : MonoBehaviour
{
    private Player player;
    public Player Player { get { return player == null ? player = FindObjectOfType<Player>() : player; } }

    private Collider coll;
    public Collider Collider { get { return coll == null ? coll = GetComponent<Collider>() : coll; } }

    private Rigidbody rb;
    public Rigidbody Rigidbody { get { return rb == null ? rb = GetComponent<Rigidbody>() : rb; } }

    [Header("-- REFERENCES --")]
    internal ChildZipperMovement childZipperMovement;
    internal ChildZipperCollision childZipperCollision;

    [Header("-- SETUP --")]
    [SerializeField] private float speed = 5f;
    [SerializeField, Tooltip("Distance between this and follow target object.")] private float followDistance = 2f;
    private int row = 1;
    private Transform followTarget;

    [Header("-- DETACH SETUP --")]
    [SerializeField, Tooltip("Force that will be applied when this zipper is detached.")] private float detachmentForce = 50f;

    public int Row => row;
    public float FollowDistance => followDistance;
    public float Speed => speed;
    public Transform FollowTarget => followTarget;
    public bool IsDetached { get; set; }
    public float DetachmentForce => detachmentForce;

    public event Action OnActivateInnerZipperPair;

    private void OnEnable()
    {
        childZipperMovement = GetComponent<ChildZipperMovement>();
        childZipperCollision = GetComponent<ChildZipperCollision>();

        IsDetached = Rigidbody.isKinematic = false;
        Collider.enabled = true;
    }

    private void OnDisable()
    {
        OnActivateInnerZipperPair = null;
    }

    public void SetRow(int row) => this.row = row;
    public void SetFollowTarget()
    {
        // if child is in the first row, follow player movement.
        if (Row == 1)
            followTarget = Player.transform;
        else
            followTarget = Player.ChildZippers[Row - 2].transform;
    }
    public void PickUpTrigger(CollectableEffect effect) => Player.PickUpTrigger(effect);

    public void Detach()
    {
        IsDetached = Rigidbody.isKinematic = true;
        Collider.enabled = childZipperMovement.enabled = false;

        OnActivateInnerZipperPair?.Invoke();
    }

    private void ApplyRandomForce()
    {
        Rigidbody.AddForce(GenerateRandomForce() * detachmentForce, ForceMode.Impulse);
    }

    private Vector3 GenerateRandomForce() => new Vector3(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(1f, 2f), UnityEngine.Random.Range(-1f, 1f));
}
