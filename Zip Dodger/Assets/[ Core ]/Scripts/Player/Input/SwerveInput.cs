using UnityEngine;

[RequireComponent(typeof(Player))]
public class SwerveInput : MonoBehaviour
{
    private Player player;

    [Header("-- SETUP --")]
    [SerializeField, Tooltip("Threshold that will ignore taking horizontal input.")] private float activationThreshold = 1f;
    [SerializeField, Tooltip("Time that if player doesn't move finger, it activates swerve restriction again for them to choose new swerve.")] private float restrictionActivationTime = 1f;
    private float restrictionActivationCounter;
    private float firstFrameFingerPositionX;
    private float lastFrameFingerPositionX;
    private float moveFactorX;
    //public float SwerveAmount { get; private set; }
    public float MoveFactorX => moveFactorX;
    public bool CanTakeInput => Mathf.Abs(firstFrameFingerPositionX - lastFrameFingerPositionX) >= activationThreshold /*&& !Player.SwervingVertically*/;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    
    private void Update()
    {
        if (!player.IsControllable) return;

        if (player.IsOnShortPlatform || Player.FinishedPlatform)
        {
            moveFactorX = 0;
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                firstFrameFingerPositionX = Input.mousePosition.x;
                lastFrameFingerPositionX = Input.mousePosition.x;
            }
            else if (Input.GetMouseButton(0))
            {
                //if (!Player.SwervingVertically) Player.SwervingHorizontally = true;

                moveFactorX = Input.mousePosition.x - lastFrameFingerPositionX;
                lastFrameFingerPositionX = Input.mousePosition.x;

                //ActivateSwerveRestriction();
                if (Mathf.Abs(moveFactorX) <= 0.05f)
                    firstFrameFingerPositionX = lastFrameFingerPositionX;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                moveFactorX = 0f;
                Player.SwervingHorizontally = false;
            }

            ActivateInputAfterThreshold();
        }
    }

    private void ActivateInputAfterThreshold()
    {
        if (CanTakeInput)
        {
            Player.SwervingHorizontally = true;
            restrictionActivationCounter = Time.time + restrictionActivationTime;
        }
        else
            moveFactorX = 0;
    }

    private void ActivateSwerveRestriction()
    {
        if (Mathf.Abs(moveFactorX) <= 0.05f && Time.time >= restrictionActivationCounter)
        {
            Player.SwervingVertically = false;
            moveFactorX = 0;

            restrictionActivationCounter = Time.time + restrictionActivationTime;
        }
    }
}
