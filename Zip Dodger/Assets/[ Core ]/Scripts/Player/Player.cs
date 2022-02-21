using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [Header("-- COMPONENTS --")]
    internal Animator animator;
    internal Collider coll;
    internal Rigidbody rb;

    [Header("-- SCRIPT REFERENCES --")]
    internal SwerveInput swerveInput;
    internal JoystickInput joystickInput;
    internal PlayerCollision playerCollision;

    [Header("-- MOVEMENT SCRIPT REFERENCES --")]
    internal PlayerRBMovement playerRBMovement;
    internal PlayerChrMovement playerChrMovement;
    internal PlayerAnimMovement playerAnimMovement;
    internal SwerveMovement swerveMovement;

    [Header("-- MOVEMENT SETUP --")]
    [SerializeField] private float maxMovementSpeed = 3f;
    [SerializeField] private float minMovementSpeed = 1f;
    [SerializeField, Range(0.1f, 3f)] private float accelerationRate = 0.5f;
    [SerializeField] private float turnSmoothTime = 0.5f;
    [SerializeField] private float jumpForce = 50f;
    [SerializeField] private float jumpCooldown = 2f;
    private float currentMovementSpeed = 1f;

    [Header("-- SWERVE MOVEMENT SETUP --")]
    [SerializeField] private float swerveSpeed = 0.5f;
    [SerializeField] private float maxSwerveAmount = 1f;

    [Header("-- GROUNDED SETUP --")]
    [SerializeField, Tooltip("Select layers that you want player to be grounded.")] private LayerMask groundLayerMask;
    [SerializeField, Tooltip("Height that player will be considered grounded when above groundable layers.")] private float groundedHeightLimit = 0.1f;

    #region Properties

    public float CurrentMovementSpeed => currentMovementSpeed;
    public float SwerveSpeed => swerveSpeed;
    public float MaxSwerveAmount => maxSwerveAmount;
    public float TurnSmoothTime => turnSmoothTime;
    public float JumpForce => jumpForce;
    public float JumpCooldown => jumpCooldown;
    public Vector3 AirVelocity { get; set; }
    public Vector3 CurrentVelocity => IsGrounded() ? rb.velocity : AirVelocity;

    // Controls
    public bool IsControllable => GameManager.GameState == GameState.Started && !IsDead;
    public bool CanJump => IsControllable && IsGrounded();
    public bool IsDead { get; private set; }
    public bool IsLanded { get; private set; }

    #endregion

    public event Action OnKill, OnJump, OnLand;
    public event Action<CollectableEffect> OnPickedUpSomething;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider>();
        TryGetComponent(out rb); // Maybe it uses character controller, not rigidbody.

        joystickInput = GetComponent<JoystickInput>();
        playerCollision = GetComponent<PlayerCollision>();

        // Input Components
        TryGetComponent(out swerveInput);
        TryGetComponent(out joystickInput);

        // Movement Components
        TryGetComponent(out playerRBMovement);
        TryGetComponent(out playerChrMovement);
        TryGetComponent(out playerAnimMovement);
        TryGetComponent(out swerveMovement);

        IsDead = false;
        IsLanded = true;
        currentMovementSpeed = minMovementSpeed;
    }

    private void OnEnable() => CharacterPositionHolder.PlayerInScene = this;

    private void Start()
    {
        OnKill += () => IsDead = true;
        OnJump += () => IsLanded = false;
        OnLand += () => IsLanded = true;

        playerCollision.OnHitSomethingBack += () => currentMovementSpeed = minMovementSpeed;
    }

    private void OnDisable()
    {
        OnKill -= () => IsDead = true;
        OnJump -= () => IsLanded = false;
        OnLand -= () => IsLanded = true;

        playerCollision.OnHitSomethingBack -= () => currentMovementSpeed = minMovementSpeed;
    }

    private void Update()
    {
        if (!IsMoving() && IsGrounded() && rb) rb.velocity = Vector3.zero;

        UpdateCurrentMovementSpeed();
    }

    private void UpdateCurrentMovementSpeed()
    {
        if (IsMoving())
            currentMovementSpeed = Mathf.MoveTowards(currentMovementSpeed, maxMovementSpeed, accelerationRate * Time.deltaTime);
        else
            currentMovementSpeed = minMovementSpeed;
    }

    public bool IsMoving()
    {
        if (joystickInput)
            return joystickInput.InputValue.magnitude > 0.05f;
        else if (swerveInput)
            return swerveInput.SwerveAmount > 0.01f;
        else
            return false;
    }

    public bool IsGrounded()
    {
        return joystickInput ? Physics.Raycast(coll.bounds.center, Vector3.down, coll.bounds.extents.y + groundedHeightLimit, groundLayerMask) && !joystickInput.JumpPressed : 
            Physics.Raycast(coll.bounds.center, Vector3.down, coll.bounds.extents.y + groundedHeightLimit, groundLayerMask);
    }

    public void KillTrigger() => OnKill?.Invoke();
    public void JumpTrigger() => OnJump?.Invoke();
    public void LandTrigger() => OnLand?.Invoke();
    public void PickUpTrigger(CollectableEffect effect) => OnPickedUpSomething?.Invoke(effect);
}
