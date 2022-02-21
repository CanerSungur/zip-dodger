using UnityEngine;
using ZestGames.Utility;

[RequireComponent(typeof(Player), typeof(Rigidbody))]
public class PlayerRBMovement : MonoBehaviour
{
    private Player player;
    private float cameraRotationY;
    [SerializeField] private float jumpForceDelay = 0.5f;

    private void Awake()
    {
        player = GetComponent<Player>();
        MultipleMovementScriptConflictCheck();
    }

    private void Start()
    {
        player.OnJump += () => Utils.DoActionAfterDelay(this, jumpForceDelay, DoJump);
    }

    private void OnDisable()
    {
        player.OnJump -= () => Utils.DoActionAfterDelay(this, jumpForceDelay, DoJump);
    }

    private void FixedUpdate()
    {
        if (!player ) return;

        if (!player.IsControllable || !player.IsMoving()) return;
        
        cameraRotationY = Camera.main.transform.rotation.eulerAngles.y;
        DoMovement(cameraRotationY);
        DoRotation(cameraRotationY);
    }

    private void DoMovement(float cameraRotationY)
    {
        player.rb.MovePosition(transform.position + Quaternion.Euler(0f, cameraRotationY, 0f) * player.joystickInput.InputValue * Time.fixedDeltaTime * player.CurrentMovementSpeed);
    }

    private void DoRotation(float cameraRotationY)
    {
        Vector3 direction = (Quaternion.Euler(0f, cameraRotationY, 0f) * player.joystickInput.InputValue).normalized;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, targetAngle, transform.rotation.eulerAngles.z), player.TurnSmoothTime);
    }

    private void DoJump()
    {
        player.rb.AddForce(new Vector3(0f, player.JumpForce, 0f), ForceMode.Impulse);
    }

    private void MultipleMovementScriptConflictCheck()
    {
        if (GetComponent<PlayerAnimMovement>() != null) throw new System.Exception($"You cannot add PlayerAnimMovement script to the {name} object while you have PlayerRBMovement script attached!");
        if (GetComponent<PlayerChrMovement>() != null) throw new System.Exception($"You cannot add PlayerChrMovement script to the {name} object while you have PlayerRBMovement script attached!");
    }
}
