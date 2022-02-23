using UnityEngine;

[RequireComponent(typeof(Gap))]
public class GapInput : MonoBehaviour
{
    private Gap gap;

    [Header("-- SETUP --")]
    [SerializeField, Tooltip("Threshold that will ignore taking vertical input to prevent opening zippers by mistake.")] private float activationThreshold = 1f;
    private float firstFrameFingerPositionY;
    private float lastFrameFingerPositionY;
    private float moveFactorY;
    public float SwerveAmount { get; private set; }
    public bool CanTakeInput => Mathf.Abs(firstFrameFingerPositionY - lastFrameFingerPositionY) >= activationThreshold;

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
            moveFactorY = Input.mousePosition.y - lastFrameFingerPositionY;
            lastFrameFingerPositionY = Input.mousePosition.y;
        }
        else if (Input.GetMouseButtonUp(0))
            moveFactorY = 0f;

        if (CanTakeInput)
        {
            SwerveAmount = moveFactorY * gap.SwerveSpeed * Time.deltaTime;
            SwerveAmount = Mathf.Clamp(SwerveAmount, -gap.MaxSwerveAmount, gap.MaxSwerveAmount);
        }
        else
        {
            SwerveAmount = 0;
        }
    }
}
