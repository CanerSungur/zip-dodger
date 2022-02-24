using UnityEngine;
using TMPro;

[RequireComponent(typeof(Gap))]
public class GapMovement : MonoBehaviour
{
    private Gap gap;
    
    [Header("-- TESTING --")]
    public TextMeshProUGUI AffectText;

    [Header("-- SETUP --")]
    [SerializeField, Tooltip("Speed of opening and closing of zipper pairs.")] private float distanceChangeSpeed = 10f;
    [SerializeField, Tooltip("Fully closed distance of zipper pairs.")] private float defaultDistance = 1.5f;
    [SerializeField, Tooltip("Fully opened distance of zipper pairs. This will change according to the zip length.")] private float maxDistance = 3f;
    private float defaultMaxDistance;

    private float affect = 0f;
    public float Affect => affect;

    private float affectLimit; // half of the zipline will be affected.
    public float AffectLimit => affectLimit;

    public float DistanceChangeSpeed => distanceChangeSpeed;
    public float DefaultDistance => defaultDistance;
    public float MaxDistance => maxDistance;

    private void Awake()
    {
        gap = GetComponent<Gap>();

        defaultMaxDistance = maxDistance;
        affect = Player.CurrentRow + 3;
        affectLimit = (int)(Player.CurrentRow * 0.5f) + 1;
    }

    private void Start()
    {
        gap.Player.OnPickedUpSomething += UpdateLimitsUponPickUp;
        gap.Player.OnDetachZipper += UpdateLimitsUponDetachZipper;
    }

    private void OnDisable()
    {
        if (gap.Player)
        {
            gap.Player.OnPickedUpSomething -= UpdateLimitsUponPickUp;
            gap.Player.OnDetachZipper -= UpdateLimitsUponDetachZipper;
        }
    }

    private void Update()
    {
        if (!gap.IsControllable) return;

        // Keeping affect value between the zip line length.
        if (affect < -Player.CurrentRow)
            affect = Player.CurrentRow + 3;
        else if (affect > Player.CurrentRow + 3)
            affect = -Player.CurrentRow;

        float newAffect = affect + gap.input.SwerveAmount;
        affect = Mathf.Lerp(affect, newAffect, gap.Speed * Time.deltaTime);

        AffectText.text = affect.ToString();
    }

    /// <summary>
    /// Changes affected zipper count and distance gap according to zip line length.
    /// </summary>
    /// <param name="obj"></param>
    private void UpdateLimitsUponPickUp(CollectableEffect obj)
    {
        if (obj != CollectableEffect.SpawnZipper) return;

        CalculateLimits();
    }

    private void UpdateLimitsUponDetachZipper(int ignoreThis) => CalculateLimits();

    private void CalculateLimits()
    {
        affectLimit = (int)(Player.CurrentRow * 0.5f) + 1;
        maxDistance = defaultMaxDistance + Player.CurrentRow;
    }
}
