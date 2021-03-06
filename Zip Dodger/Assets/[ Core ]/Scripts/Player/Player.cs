using UnityEngine;
using System;
using System.Collections.Generic;

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
    [SerializeField] private bool useAcceleration;
    [SerializeField] private float maxMovementSpeed = 3f;
    [SerializeField] private float minMovementSpeed = 1f;
    [SerializeField, Range(0.1f, 3f)] private float accelerationRate = 0.5f;
    [SerializeField] private float turnSmoothTime = 0.5f;
    [SerializeField] private float jumpForce = 50f;
    [SerializeField] private float jumpCooldown = 2f;
    [SerializeField, Tooltip("Min and max limit of x axis movement.")] private Vector2 horizontalMovementLimit;
    private float currentMovementSpeed = 1f;

    [Header("-- SWERVE MOVEMENT SETUP --")]
    [SerializeField] private float swerveSpeed = 0.5f;
    [SerializeField] private float maxSwerveAmount = 1f;

    [Header("-- GROUNDED SETUP --")]
    [SerializeField, Tooltip("Select layers that you want player to be grounded.")] private LayerMask groundLayerMask;
    [SerializeField, Tooltip("Height that player will be considered grounded when above groundable layers.")] private float groundedHeightLimit = 0.1f;

    [Header("-- ZIPPER SETUP --")]
    [SerializeField] private float detachmentForce;
    //private static int currentRow = 0;
    private List<ChildZipper> childZippers = new List<ChildZipper>();
    public List<ChildZipper> ChildZippers => childZippers;

    #region Properties

    public static int CurrentRow { get; set; }
    public float DetachmentForce => detachmentForce;
    public float CurrentMovementSpeed => currentMovementSpeed;
    public float SwerveSpeed => swerveSpeed;
    public float MaxSwerveAmount => maxSwerveAmount;
    public float TurnSmoothTime => turnSmoothTime;
    public float JumpForce => jumpForce;
    public float JumpCooldown => jumpCooldown;
    public Vector3 AirVelocity { get; set; }
    public Vector3 CurrentVelocity => IsGrounded() ? rb.velocity : AirVelocity;
    public Vector2 HorizontalMovementLimit => horizontalMovementLimit;

    // Controls
    public bool IsControllable => GameManager.GameState == GameState.Started && !IsDead;
    public bool CanJump => IsControllable && IsGrounded();
    public bool IsDead { get; private set; }
    public bool IsLanded { get; private set; }

    public static bool SwervingHorizontally, SwervingVertically;
    public bool IsOnShortPlatform = false;
    public static bool FinishedPlatform { get; private set; }

    #endregion

    public event Action OnKill, OnJump, OnLand;
    public event Action<CollectableEffect> OnPickedUpSomething;
    public event Action<int> OnDetachZipper;

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
        if (useAcceleration)
            currentMovementSpeed = minMovementSpeed;
        else
            currentMovementSpeed = maxMovementSpeed;

        childZippers.Clear();

        CurrentRow = 0;

        SwervingHorizontally = SwervingVertically = FinishedPlatform = false;
    }

    private void OnEnable() => CharacterPositionHolder.PlayerInScene = this;

    private void Start()
    {
        OnKill += () => IsDead = true;
        OnJump += () => IsLanded = false;
        OnLand += () => IsLanded = true;
        OnDetachZipper += DetachZipper;
        GameManager.OnPlatformEnd += () => FinishedPlatform = true;
        GameManager.OnPlatformEnd += IncreaseCurrentSpeed;

        playerCollision.OnHitSomethingBack += () => { if (useAcceleration) currentMovementSpeed = minMovementSpeed; };
    }

    private void OnDisable()
    {
        OnKill -= () => IsDead = true;
        OnJump -= () => IsLanded = false;
        OnLand -= () => IsLanded = true;
        OnDetachZipper -= DetachZipper;
        GameManager.OnPlatformEnd -= () => FinishedPlatform = true;
        GameManager.OnPlatformEnd -= IncreaseCurrentSpeed;

        playerCollision.OnHitSomethingBack -= () => { if (useAcceleration) currentMovementSpeed = minMovementSpeed; };

        OnKill = OnJump = OnLand = null;
        OnPickedUpSomething = null;
        OnDetachZipper = null;
    }

    private void Update()
    {
        if (!IsMoving() && IsGrounded() && rb) rb.velocity = Vector3.zero;

        if (useAcceleration)
            UpdateCurrentMovementSpeed();
    }

    public void DetachZipper(int row)
    {
        for (int i = childZippers.Count - 1; i >= row - 1; i--)
        {
            childZippers[i].Detach();
            childZippers.RemoveAt(i);
            //DecreaseCurrentRow();
            CurrentRow--;
        }
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
            return swerveInput.MoveFactorX > 0.01f;
        else
            return false;
    }

    public bool IsGrounded()
    {
        return joystickInput ? Physics.Raycast(coll.bounds.center, Vector3.down, coll.bounds.extents.y + groundedHeightLimit, groundLayerMask) && !joystickInput.JumpPressed :
            Physics.Raycast(coll.bounds.center, Vector3.down, coll.bounds.extents.y + groundedHeightLimit, groundLayerMask);
    }

    private void IncreaseCurrentSpeed() => currentMovementSpeed *= 1.5f;

    public void KillTrigger() => OnKill?.Invoke();
    public void JumpTrigger() => OnJump?.Invoke();
    public void LandTrigger() => OnLand?.Invoke();
    public void PickUpTrigger(CollectableEffect effect) => OnPickedUpSomething?.Invoke(effect);
    public void DetachZipperTrigger(int row) => OnDetachZipper?.Invoke(row);
    //public void IncreaseCurrentRow() => currentRow++;
    //public void DecreaseCurrentRow() => currentRow--;
}
