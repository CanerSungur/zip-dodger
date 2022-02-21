using UnityEngine;
using UnityEngine.AI;
using ZestGames.Utility;

[RequireComponent(typeof(NavMeshAgent))]
public class AI : MonoBehaviour
{
    public enum Side { Ally, Enemy, Neutral }
    [Header("--  --")]
    [Tooltip("Is it Ally? Enemy? or Neutral?")] public Side _Side;

    [Header("-- COMPONENTS --")]
    internal Animator animator;
    internal Collider coll;
    internal Rigidbody rb;
    internal NavMeshAgent navMeshAgent;

    [Header("-- REFERENCES --")]
    internal AINavMeshMovement navMeshMovement;
    internal AIAnimationController animationController;
    internal AICollision aiCollision;
    internal AIAction aiAction;

    [Header("-- MOVEMENT SETUP --")]
    [SerializeField] private float maxMovementSpeed = 3f;
    [SerializeField] private float minMovementSpeed = 1f;
    [SerializeField, Range(0.1f, 3f)] private float accelerationRate = 0.5f;
    private float currentMovementSpeed = 1f;

    [Header("-- GROUNDED SETUP --")]
    [SerializeField, Tooltip("Select layers that you want player to be grounded.")] private LayerMask groundLayerMask;
    [SerializeField, Tooltip("Height that player will be considered grounded when above groundable layers.")] private float groundedHeightLimit = 0.1f;

    #region Properties

    public Vector3 CurrentVelocity => navMeshAgent.velocity;
    public LayerMask GroundLayerMask => groundLayerMask;

    // Controls
    public bool CanMove => GameManager.GameState == GameState.Started && aiAction.Target;
    public bool IsMoving => navMeshAgent.velocity.magnitude > 0.1f;
    public bool IsGrounded => Physics.Raycast(coll.bounds.center, Vector3.down, coll.bounds.extents.y + groundedHeightLimit, groundLayerMask);
    public bool IsDead { get; private set; }
    public float CurrentMovementSpeed => currentMovementSpeed;

    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshMovement = GetComponent<AINavMeshMovement>();
        animationController = GetComponent<AIAnimationController>();
        aiCollision = GetComponent<AICollision>();
        aiAction = GetComponent<AIAction>();

        currentMovementSpeed = minMovementSpeed;
    }

    private void OnEnable() => CharacterPositionHolder.AddAI(this);
    private void OnDisable() => CharacterPositionHolder.RemoveAI(this);

    private void Start()
    {
        IsDead = false;
    }

    private void Update()
    {
        if (!IsMoving && IsGrounded && rb) rb.velocity = Vector3.zero;

        UpdateCurrentMovementSpeed();

        if (Input.GetKeyDown(KeyCode.Space))
            Die();
    }

    private void UpdateCurrentMovementSpeed()
    {
        if (IsMoving)
            currentMovementSpeed = Mathf.MoveTowards(currentMovementSpeed, maxMovementSpeed, accelerationRate * Time.deltaTime);
        else
            currentMovementSpeed = minMovementSpeed;
    }

    private void Die()
    {
        IsDead = true;
        CharacterPositionHolder.RemoveAI(this);
        CollectableManager.SpawnCoinRewardsTrigger(transform.position, 5);

        Utils.DoActionAfterDelay(this, 2f, () => gameObject.SetActive(false));
    }    
}
