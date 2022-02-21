using UnityEngine;
using System;

[RequireComponent(typeof(Player))]
public class JoystickInput : MonoBehaviour
{
    private Player player;

    [Header("-- INPUT SETUP --")]
    [SerializeField] private Joystick joystick;

    public Vector3 InputValue { get; private set; }
    private bool jumpPressed;
    public bool JumpPressed => jumpPressed;

    private float jumpTimer;

    private void Awake()
    {
        player = GetComponent<Player>();
        jumpPressed = false;
        jumpTimer = player.JumpCooldown;
    }

    private void Update()
    {
        if (!player) return;

        if (Input.GetKeyDown(KeyCode.Space) && player.CanJump && !jumpPressed)
        {
            player.JumpTrigger();
            jumpPressed = true;
        }

        if (jumpPressed)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0f)
            {
                jumpTimer = player.JumpCooldown;
                jumpPressed = false;
            }
        }

        if (player.IsControllable)
            InputValue = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
        else
            InputValue = Vector3.zero;
    }
}
