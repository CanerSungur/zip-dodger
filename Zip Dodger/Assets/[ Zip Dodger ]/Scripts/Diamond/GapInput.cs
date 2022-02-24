using UnityEngine;

[RequireComponent(typeof(Gap))]
public class GapInput : MonoBehaviour
{
    private Gap gap;

    [Header("-- SETUP --")]
    [SerializeField, Tooltip("Threshold that will ignore taking vertical input to prevent opening zippers by mistake.")] private float activationThreshold = 1f;
    [SerializeField, Tooltip("Time that if player doesn't move finger, it activates swerve restriction again for them to choose new swerve.")] private float restrictionActivationTime = 1f;
    private float restrictionActivationCounter;
    private float firstFrameFingerPositionY;
    private float lastFrameFingerPositionY;
    private float moveFactorY;
    public float SwerveAmount { get; private set; }
    public bool CanTakeInput => Mathf.Abs(firstFrameFingerPositionY - lastFrameFingerPositionY) >= activationThreshold /*&& !Player.SwervingHorizontally*/;

    private void Awake()
    {
        gap = GetComponent<Gap>();
    }

    private void Update()
    {
        if (!gap.IsControllable) return;

        if (Input.GetMouseButtonDown(0))
        {
            firstFrameFingerPositionY = Input.mousePosition.y;
            lastFrameFingerPositionY = Input.mousePosition.y;
        }
        else if (Input.GetMouseButton(0))
        {
            //if (!Player.SwervingHorizontally) Player.SwervingVertically = true;

            moveFactorY = Input.mousePosition.y - lastFrameFingerPositionY;
            lastFrameFingerPositionY = Input.mousePosition.y;

            //ActivateSwerveRestriction();
            if (Mathf.Abs(moveFactorY) <= 0.05f)
                firstFrameFingerPositionY = lastFrameFingerPositionY;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            moveFactorY = 0f;
            Player.SwervingVertically = false;
        }

        ActivateInputAfterThreshold();
    }

    private void ActivateInputAfterThreshold()
    {
        if (CanTakeInput)
        {
            Player.SwervingVertically = true;
            restrictionActivationCounter = Time.time + restrictionActivationTime;

            SwerveAmount = moveFactorY * gap.SwerveSpeed * Time.deltaTime;
            SwerveAmount = Mathf.Clamp(SwerveAmount, -gap.MaxSwerveAmount, gap.MaxSwerveAmount);
        }
        else
            SwerveAmount = 0;
    }

    private void ActivateSwerveRestriction()
    {
        if (Mathf.Abs(moveFactorY) <= 0.05f && Time.time >= restrictionActivationCounter)
        {
            Player.SwervingVertically = false;
            moveFactorY = 0;

            restrictionActivationCounter = Time.time + restrictionActivationTime;
        }
    }
}
