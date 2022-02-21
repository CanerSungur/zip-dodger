using UnityEngine;

/// <summary>
/// Makes object hover above ground.
/// If it's on some other object, it uses force down. The object under us needs to have this script too.
/// Imagine if this object jumps on the car and car is pushed down a little.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PhysicsHover : MonoBehaviour
{
    [Header("-- REFERENCES --")]
    [Tooltip("Rigidbody you want to force upright stand.")] private Rigidbody effectedRigidbody;

    [Header("-- SETUP --")]
    [SerializeField] private float rayLength = 3f;
    [SerializeField] private float rideHeight = 2.5f;
    [SerializeField] private float rideSpringStrength = 10f;
    [SerializeField] private float rideSpringDamper = 1f;
    [SerializeField, Range(0.01f, 1f), Tooltip("Percentage of spring force that is applied to the objects under this object.")] private float downwardsAppliedForceRatio = 0.5f;
    private RaycastHit rayHit;
    private bool rayDidHit = false;
    private Vector3 downDir = Vector3.down;

    private void Awake()
    {
        TryGetComponent(out effectedRigidbody);
        if (!effectedRigidbody) throw new System.Exception($"{gameObject.name} object needs to reference its rigidbody to PhysicsHover Script.");
    }

    private void FixedUpdate()
    {
        Hover();
    }

    private void ShootRayBelow()
    {
        rayDidHit = Physics.Raycast(transform.position, Vector3.down, out rayHit, rayLength);
    }

    private void Hover()
    {
        ShootRayBelow();

        if (rayDidHit)
        {
            Vector3 vel =  effectedRigidbody.velocity;
            Vector3 rayDir = transform.TransformDirection(downDir);

            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = rayHit.rigidbody;
            if (hitBody != null)
            {
                otherVel = hitBody.velocity;
            }

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;

            float x = rayHit.distance - rideHeight;

            float springForce = (x * rideSpringStrength) - (relVel * rideSpringDamper);

            Debug.DrawLine(transform.position, transform.position + (rayDir * -springForce), Color.yellow);

            effectedRigidbody.AddForce(rayDir * springForce);

            if (hitBody != null)
            {
                // Force is applied to the object under the character in the opposite direction.
                hitBody.AddForceAtPosition(rayDir * -springForce * downwardsAppliedForceRatio, rayHit.point);
            }
        }
    }
}
