using UnityEngine;

/// <summary>
/// This script forces the rigidbody to stand upright.
/// Makes object come back to original rotation. Makes getting hit more real.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PhysicsUprightStanding : MonoBehaviour
{
    [Header("-- REFERENCE --")]
    [Tooltip("Rigidbody you want to force upright stand.")] private Rigidbody effectedRigidbody;

    [Header("-- SETUP --")]
    [SerializeField] private float uprightJointSpringStrength = 10f;
    [SerializeField] private float uprightJointSpringDamper = 1f;
    private Quaternion uprightJointTargetRot = Quaternion.Euler(0, 0, 0);

    private void Awake()
    {
        TryGetComponent(out effectedRigidbody);
        if (!effectedRigidbody) throw new System.Exception($"{gameObject.name} object needs to reference its rigidbody to PhysicsHover Script.");
    }

    private void FixedUpdate()
    {
        ApplyUprightForce();
    }

    private void ApplyUprightForce()
    {
        Quaternion characterCurrent = transform.rotation;
        Quaternion toGoal = ShortestRotation(Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f), characterCurrent);

        Vector3 rotAxis;
        float rotDegrees;

        toGoal.ToAngleAxis(out rotDegrees, out rotAxis);
        rotAxis.Normalize();

        float rotRadians = rotDegrees * Mathf.Deg2Rad;

        effectedRigidbody.AddTorque((rotAxis * (rotRadians * uprightJointSpringStrength)) - (effectedRigidbody.angularVelocity * uprightJointSpringDamper));
    }

    private Quaternion ShortestRotation(Quaternion a, Quaternion b)
    {
        if (Quaternion.Dot(a, b) < 0)
        {
            return a * Quaternion.Inverse(Multiply(b, -1));
        }
        else return a * Quaternion.Inverse(b);
    }

    private Quaternion Multiply(Quaternion input, float scalar)
    {
        return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
    }
}
