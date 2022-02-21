using System;
using UnityEngine;

[RequireComponent(typeof(Player), typeof(Animator))]
public class PlayerAnimMovement : MonoBehaviour
{
    private Player player;

    private Transform _camTransform;

    // parameters needed to control character
    //bool _onGround; // Is the character on the ground
    Vector3 _moveInput;
    bool _crouch;
    bool _jump;
    float _turnAmount;
    float _forwardAmount;
    bool _enabled = true;
    protected Vector3 _airVelocity;
    protected bool _jumpPressed = false;
    protected bool _firstAnimatorFrame = true;  // needed for prevent changing position in first animation frame

    // Animator Parameters
    private readonly int _animatorGrounded = Animator.StringToHash("Base Layer.Grounded");
    private readonly int _animatorTurn = Animator.StringToHash("Turn");
    private readonly int _animatorForward = Animator.StringToHash("Forward");
    readonly int _animatorOnGround = Animator.StringToHash("OnGround");
    readonly int _animatorJump = Animator.StringToHash("Jump");
    readonly int _animatorJumpLeg = Animator.StringToHash("JumpLeg");

    // Extra tweaks for animations
    const float JumpPower = 5f;     // determines the jump force applied when jumping (and therefore the jump height)
    const float AirSpeed = 5f;      // determines the max speed of the character while airborne
    const float AirControl = 2f;    // determines the response speed of controlling the character while airborne
    const float StationaryTurnSpeed = 180f; // additional turn speed added when the player is stationary (added to animation root rotation)
    const float MovingTurnSpeed = 360f;     // additional turn speed added when the player is moving (added to animation root rotation)
    const float RunCycleLegOffset = 0.2f;   // animation cycle offset (0-1) used for determining correct leg to jump off

    private Vector3 charVel => player.IsGrounded() ? player.rb.velocity :_airVelocity;

    private void Awake()
    {
        player = GetComponent<Player>();
        MultipleMovementScriptConflictCheck();

        if (Camera.main == null)
            throw new Exception("No Main Camera Found!");
        else
            _camTransform = Camera.main.transform;
    }

    private void Start()
    {
        player.animator.applyRootMotion = false;
    }

    void FixedUpdate()
    {
        Vector3 camForward = new Vector3(_camTransform.forward.x, 0, _camTransform.forward.z).normalized;
        Vector3 move = player.joystickInput.InputValue.z * camForward + player.joystickInput.InputValue.x * _camTransform.right;
        Move(move, false, player.joystickInput.JumpPressed);
        if (move.magnitude > 1)
            move.Normalize();

        if (!_enabled)
            return;

        int currentAnimation = player.animator.GetCurrentAnimatorStateInfo(0).fullPathHash;

        ApplyExtraTurnRotation(currentAnimation);       // this is in addition to root rotation in the animations
        ConvertMoveInput();             // converts the relative move vector into local turn & fwd values

        // control and velocity handling is different when grounded and airborne:
        if (player.IsGrounded())
            HandleGroundedVelocities(currentAnimation);
        else
            HandleAirborneVelocities();

        UpdateAnimator(); // send input and other state parameters to the animator
    }

    void OnAnimatorMove()
    {
        if (Time.deltaTime < Mathf.Epsilon)
            return;

        Vector3 deltaPos;
        Vector3 deltaGravity = Physics.gravity * Time.deltaTime;
        _airVelocity += deltaGravity;

        if (player.IsGrounded())
        {
            deltaPos = player.animator.deltaPosition;
            deltaPos.y -= 5f * Time.deltaTime;
        }
        else
        {
            deltaPos = _airVelocity * Time.deltaTime;
        }

        if (_firstAnimatorFrame)
        {
            // if Animator just started, Animator move character
            // so you need to zeroing movement
            deltaPos = new Vector3(0f, deltaPos.y, 0f);
            _firstAnimatorFrame = false;
        }

        UpdatePlayerPosition(deltaPos);

        // apply animator rotation
        transform.rotation *= player.animator.deltaRotation;
        //_jumpPressed = false;
    }

    public void Move(Vector3 move, bool crouch, bool jump)
    {
        _moveInput = move;
        _crouch = crouch;
        _jump = jump;
    }

    /// <summary>
    /// Move character by specified delta vector
    /// </summary>
    private void UpdatePlayerPosition(Vector3 deltaPos)
    {
        Vector3 finalVelocity = deltaPos / Time.deltaTime;
        if (!player.joystickInput.JumpPressed)
        {
            finalVelocity.y = player.rb.velocity.y;
        }
        //else
        //{
        //    _jumpStartedTime = Time.time;
        //}
        _airVelocity = finalVelocity;       // i need this to correctly detect player velocity in air mode
         player.rb.velocity = finalVelocity;
    }

    private void HandleGroundedVelocities(int currentAnimation)
    {
        bool animationGrounded = currentAnimation == _animatorGrounded;

        // check whether conditions are right to allow a jump
        if (!(_jump & !_crouch & animationGrounded))
            return;

        // jump!
        Vector3 newVelocity = charVel;
        newVelocity.y += JumpPower;
        _airVelocity = newVelocity;

        _jump = false;
        //_onGround = false;
        _jumpPressed = true;
    }

    private void UpdateAnimator()
    {
        // Here we tell the animator what to do based on the current states and inputs.

        // update the animator parameters
        player.animator.SetFloat(_animatorForward, _forwardAmount, 0.1f, Time.deltaTime);
        player.animator.SetFloat(_animatorTurn, _turnAmount, 0.1f, Time.deltaTime);
        player.animator.SetBool(_animatorOnGround, player.IsGrounded());
        if (player.IsGrounded()) // if flying
            player.animator.SetFloat(_animatorJump, charVel.y);
        else
        {
            // calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            float runCycle = Mathf.Repeat(
                    player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime + RunCycleLegOffset, 1);

            float jumpLeg = (runCycle < 0.5f ? 1 : -1) * _forwardAmount;
            player.animator.SetFloat(_animatorJumpLeg, jumpLeg);
        }
    }
    /// <summary>
    /// Prepare input movement data for animator
    /// </summary>
    private void ConvertMoveInput()
    {
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction. 
        Vector3 localMove = transform.InverseTransformDirection(_moveInput);
        if ((Math.Abs(localMove.x) > float.Epsilon) &
            (Math.Abs(localMove.z) > float.Epsilon))
            _turnAmount = Mathf.Atan2(localMove.x, localMove.z);
        else
            _turnAmount = 0f;

        _forwardAmount = localMove.z;
    }
    /// <summary>
    /// Animation rotates very slow, so adding a little rotation will be very helpful
    /// </summary>
    private void ApplyExtraTurnRotation(int currentAnimation)
    {
        if (currentAnimation != _animatorGrounded)
            return;

        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(StationaryTurnSpeed, MovingTurnSpeed,
                                     _forwardAmount);
        transform.Rotate(0, _turnAmount * turnSpeed * Time.deltaTime, 0);
    }

    private void HandleAirborneVelocities()
    {
        // we allow some movement in air, but it's very different to when on ground
        // (typically allowing a small change in trajectory)
        Vector3 airMove = new Vector3(_moveInput.x * AirSpeed, _airVelocity.y, _moveInput.z * AirSpeed);
        _airVelocity = Vector3.Lerp(_airVelocity, airMove, Time.deltaTime * AirControl);
    }

    private void MultipleMovementScriptConflictCheck()
    {
        if (GetComponent<PlayerRBMovement>() != null) throw new System.Exception($"You cannot add PlayerRBMovement script to the {name} object while you have PlayerAnimMovement script attached!");
        if (GetComponent<PlayerChrMovement>() != null) throw new System.Exception($"You cannot add PlayerChrMovement script to the {name} object while you have PlayerAnimMovement script attached!");
    }
}
