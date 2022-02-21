using UnityEngine;

public class ChildZipper : MonoBehaviour
{
    private Player player;
    public Player Player { get { return player == null ? player = FindObjectOfType<Player>() : player; } }

    private Collider coll;
    public Collider Collider { get { return coll == null ? coll = GetComponent<Collider>() : coll; } }

    [Header("-- REFERENCES --")]
    internal ChildZipperMovement childZipperMovement;
    internal ChildZipperCollision childZipperCollision;

    [Header("-- SETUP --")]
    [SerializeField] private float speed = 5f;
    [SerializeField, Tooltip("Distance between this and follow target object.")] private float followDistance = 2f;
    private int row = 1;
    private Transform followTarget;

    public int Row => row;
    public float FollowDistance => followDistance;
    public float Speed => speed;
    public Transform FollowTarget => followTarget;

    private void OnEnable()
    {
        childZipperMovement = GetComponent<ChildZipperMovement>();
        childZipperCollision = GetComponent<ChildZipperCollision>();
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
}
