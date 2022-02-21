using UnityEngine;
using System;

[RequireComponent(typeof(AI))]
public class AICollision : MonoBehaviour
{
    private AI ai;

    public event Action OnHitSomethingFront, OnHitSomethingBack;

    private void Awake()
    {
        ai = GetComponent<AI>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == ai.GroundLayerMask) return;

        Vector3 relativePosition = transform.InverseTransformPoint(collision.GetContact(0).point);

        if (relativePosition.z > 0)
        {
            //print("The object is in front.");
            OnHitSomethingFront?.Invoke();
        }
        else
        {
            //print("The object is behind.");
            OnHitSomethingBack?.Invoke();
        }
    }
}
