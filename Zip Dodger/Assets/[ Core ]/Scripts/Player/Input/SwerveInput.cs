using UnityEngine;

[RequireComponent(typeof(Player))]
public class SwerveInput : MonoBehaviour
{
    private Player player;

    [Header("-- SETUP --")]
    private float lastFrameFingerPositionX;
    private float moveFactorX;
    public float SwerveAmount { get; private set; }

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    
    private void Update()
    {
        if (!player.IsControllable) return;

        if (Input.GetMouseButtonDown(0))
            lastFrameFingerPositionX = Input.mousePosition.x;
        else if (Input.GetMouseButton(0))
        {
            moveFactorX = Input.mousePosition.x - lastFrameFingerPositionX;
            lastFrameFingerPositionX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
            moveFactorX = 0f;

        SwerveAmount = moveFactorX * player.SwerveSpeed * Time.deltaTime;
        SwerveAmount = Mathf.Clamp(SwerveAmount, -player.MaxSwerveAmount, player.MaxSwerveAmount);
    }
}
