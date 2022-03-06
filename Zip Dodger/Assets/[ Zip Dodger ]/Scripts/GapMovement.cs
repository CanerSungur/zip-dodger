using UnityEngine;

[RequireComponent(typeof(Gap))]
public class GapMovement : MonoBehaviour
{
    private Gap gap;

    [Header("-- SETUP --")]
    [SerializeField, Tooltip("Max number of zippers who will be affected when gaping.")] int maxAffectLimit = 6;
    [SerializeField, Tooltip("Speed of opening and closing of zipper pairs.")] float distanceChangeSpeed = 10f;
    [SerializeField, Tooltip("Fully closed distance of zipper pairs.")] float defaultDistance = 1.5f;
    [SerializeField, Tooltip("Fully opened distance of zipper pairs. This will change according to the zip length.")] float maxDistance = 3f;
    float defaultMaxDistance;

    float affect = 0f;
    public float Affect => affect;

    int affectLimit; // half of the zipline will be affected.
    public int AffectLimit
    {
        get
        {
            if (affectLimit < 0)
                return 0;
            else if (affectLimit >= maxAffectLimit)
                return maxAffectLimit;
            else
                return affectLimit;
        }
    }

    public float DistanceChangeSpeed => distanceChangeSpeed;
    public float DefaultDistance => defaultDistance;
    public float MaxDistance => maxDistance;

    void Awake()
    {
        gap = GetComponent<Gap>();

        defaultMaxDistance = maxDistance;
        affect = Player.CurrentRow + 3;
        affectLimit = (int)(Player.CurrentRow * 0.5f) + 1;
    }

    void Start()
    {
        gap.Player.OnPickedUpSomething += UpdateLimitsUponPickUp;
        gap.Player.OnDetachZipper += UpdateLimitsUponDetachZipper;
    }

    void OnDisable()
    {
        if (gap.Player)
        {
            gap.Player.OnPickedUpSomething -= UpdateLimitsUponPickUp;
            gap.Player.OnDetachZipper -= UpdateLimitsUponDetachZipper;
        }
    }

    void Update()
    {
        if (!gap.IsControllable) return;

        KeepAffectValueInBetween();

        float newAffect = affect + gap.input.SwerveAmount;

        if (Player.FinishedPlatform)
            affect = Mathf.Lerp(affect, 99, gap.Speed * Time.deltaTime);
        else
            affect = Mathf.Lerp(affect, newAffect, gap.Speed * Time.deltaTime);
    }

    /// <summary>
    /// Changes affected zipper count and distance gap according to zip line length.
    /// </summary>
    /// <param name="obj"></param>
    void UpdateLimitsUponPickUp(CollectableEffect obj)
    {
        if (obj != CollectableEffect.SpawnZipper) return;

        CalculateLimits();
    }

    void UpdateLimitsUponDetachZipper(int ignoreThis) => CalculateLimits();

    void CalculateLimits()
    {
        affectLimit = (int)(Player.CurrentRow * 0.5f) + 1;
        //maxDistance = defaultMaxDistance + Player.CurrentRow;
    }

    void KeepAffectValueInBetween()
    {
        if (!Player.FinishedPlatform)
        {
            // Keeping affect value between the zip line length.
            //if (affect < -Player.CurrentRow)
            //    affect = Player.CurrentRow + 3;
            //else if (affect > Player.CurrentRow + 3)
            //    affect = -Player.CurrentRow;
            if (Player.CurrentRow == 0)
            {
                if (affect < -2f)
                    affect = 4f;
                else if (affect > 4f)
                    affect = -2f;
            }
            else
            {
                if (affect < -Player.CurrentRow)
                    affect = Player.CurrentRow * 2f;
                else if (affect > Player.CurrentRow * 2f)
                    affect = -Player.CurrentRow;
            }
        }
    }
}
