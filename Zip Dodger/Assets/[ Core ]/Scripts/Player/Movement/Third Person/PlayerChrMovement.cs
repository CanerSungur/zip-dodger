using UnityEngine;

[RequireComponent(typeof(Player), typeof(CharacterController))]
public class PlayerChrMovement : MonoBehaviour
{
    private Player player;
    private CharacterController controller;

    private void Awake()
    {
        player = GetComponent<Player>();
        controller = GetComponent<CharacterController>();
        MultipleMovementScriptConflictCheck();
    }

    [Header("Controller Setup")]
    private Vector3 playerVelocity; // velocity of character controller. Not the rigidbody of player.
    private const float gravityValue = -9.81f;

    private void Start()
    {
        #region Character Controller Setup

        //Controller.stepOffset = 0.3f;
        //Controller.skinWidth = 0.08f;
        //Controller.center = new Vector3(0.04f, 0.95f, 0f);
        //Controller.radius = 0.3f;
        //Controller.height = 1.7f;

        #endregion
    }

    private void Update()
    {
        if (player.IsGrounded() && playerVelocity.y < 0f)
            playerVelocity.y = 0f;

        DoMovement();
    }

    private void DoMovement()
    {
        controller.Move(player.joystickInput.InputValue * Time.deltaTime * player.CurrentMovementSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void DoJump()
    {
        // TODO: Inverse gravity to fake jump.
    }

    private void MultipleMovementScriptConflictCheck()
    {
        if (GetComponent<PlayerAnimMovement>() != null) throw new System.Exception($"You cannot add PlayerAnimMovement script to the {name} object while you have PlayerChrMovement script attached!");
        if (GetComponent<PlayerRBMovement>() != null) throw new System.Exception($"You cannot add PlayerRBMovement script to the {name} object while you have PlayerChrMovement script attached!");
    }
}
