using UnityEngine;

[RequireComponent(typeof(Player), typeof(SwerveInput), typeof(Rigidbody))]
public class SwerveMovement : MonoBehaviour
{
    private Player player;

    [Header("-- ROTATION SETUP --")]
    private Quaternion maxRightRotation = Quaternion.Euler(5f, 5f, -10f);
    private Quaternion maxLeftRotation = Quaternion.Euler(5f, -5f, 10f);
    private Quaternion defaultRotation = Quaternion.identity;
    private float rotationSpeed;
    private bool goLeft = false;
    private bool goRight = false;

    private Vector3 firstMousePos, currentMousePos;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        if (!player.IsControllable) return;

        if (GameManager.GameState != GameState.PlatformIsOver)
        {
            if (Input.GetMouseButton(0))
            {
                CheckTurnDirection();
            }
            else
                goLeft = goRight = false;

            //TurnPlayer();
        }

        if (player.IsGrounded())
        {
            float swerveAmount = Time.fixedDeltaTime * player.SwerveSpeed * player.swerveInput.MoveFactorX;
            swerveAmount = Mathf.Clamp(swerveAmount, -player.MaxSwerveAmount, player.MaxSwerveAmount);

                Vector3 newPos = transform.position + new Vector3(swerveAmount, 0f, 0f);
            if (player.IsOnShortPlatform || Player.FinishedPlatform)
                transform.position = Vector3.Lerp(transform.position, new Vector3(0f, transform.position.y, transform.position.z), player.CurrentMovementSpeed * Time.fixedDeltaTime);
            else
                transform.position = Vector3.Lerp(transform.position, newPos, player.CurrentMovementSpeed * Time.fixedDeltaTime);

            ApplyLimitToPositionX(player.HorizontalMovementLimit.x, player.HorizontalMovementLimit.y);
            player.rb.velocity = Vector3.forward * player.CurrentMovementSpeed;
        }
    }

    private void ApplyLimitToPositionX(float min, float max) => transform.position = new Vector3(Mathf.Clamp(transform.position.x, min, max), transform.position.y, transform.position.z);

    private void CheckTurnDirection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstMousePos = currentMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
            currentMousePos = Input.mousePosition;

        if (currentMousePos.x < firstMousePos.x)
        {
            goLeft = true;
            goRight = false;
        }
        else if (currentMousePos.x > firstMousePos.x)
        {
            goLeft = false;
            goRight = true;
        }
        else
        {
            goLeft = false;
            goRight = false;
        }
    }

    private void TurnPlayer()
    {
        rotationSpeed = player.CurrentMovementSpeed / 50f;

        if (goLeft && !goRight)
            transform.rotation = Quaternion.Lerp(transform.rotation, maxLeftRotation, rotationSpeed);
        else if (!goLeft && goRight)
            transform.rotation = Quaternion.Lerp(transform.rotation, maxRightRotation, rotationSpeed);
        else
            transform.rotation = Quaternion.Lerp(transform.rotation, defaultRotation, 0.1f);
    }

    private void Stop()
    {
        player.rb.velocity = Vector3.zero;
    }
    private void ContinueRunning()
    {
        player.rb.velocity = Vector3.forward * player.CurrentMovementSpeed;
    }

    private void ResetTransform()
    {
        transform.position = new Vector3(0f, transform.position.y, transform.position.z);
        transform.rotation = Quaternion.identity;
    }

}
