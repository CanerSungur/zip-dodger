using System;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerAnimationController : MonoBehaviour
{
    private Player player;

	[Header("-- ANIMATION NAME SETUP --")]
    private readonly int turnID = Animator.StringToHash("Turn");
    private readonly int forwardID = Animator.StringToHash("Forward");
    private readonly int jumpID = Animator.StringToHash("Jump");
    private readonly int onGroundID = Animator.StringToHash("OnGround");

    private float turnAmount, forwardAmount;
    private Transform camTransform;

	private void Awake()
    {
        player = GetComponent<Player>();
        camTransform = Camera.main.transform;
    }

    private void Start()
    {
        player.OnJump += Jump;
    }

    private void OnDisable()
    {
        player.OnJump-= Jump;
    }

    private void FixedUpdate()
    {
        Vector3 camForward = new Vector3(camTransform.forward.x, 0, camTransform.forward.z).normalized;
        Vector3 move = player.joystickInput.InputValue.z * camForward + player.joystickInput.InputValue.x * camTransform.right;
        if (move.magnitude > 1)
            move.Normalize();

        ConvertMoveInput(move);
        UpdateAnimator();

        GroundCheck();
    }

    private void UpdateAnimator()
    {
        player.animator.SetFloat(forwardID, forwardAmount, 0.1f, Time.deltaTime);
        player.animator.SetFloat(turnID, turnAmount, 0.1f, Time.deltaTime);
    }

    /// <summary>
    /// Prepare input movement data for animator
    /// </summary>
    private void ConvertMoveInput(Vector3 input)
    {
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction. 
        Vector3 localMove = transform.InverseTransformDirection(input);
        if ((Math.Abs(localMove.x) > float.Epsilon) &
            (Math.Abs(localMove.z) > float.Epsilon))
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
        else
            turnAmount = 0f;

        forwardAmount = localMove.z;
    }

    private void Jump() => player.animator.SetTrigger(jumpID);
    private void GroundCheck() => player.animator.SetBool(onGroundID, player.IsGrounded());
}
