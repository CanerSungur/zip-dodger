using UnityEngine;
using System;

[RequireComponent(typeof(Player))]
public class PlayerCollision : MonoBehaviour
{
    private Player player;

    public event Action OnHitSomethingFront, OnHitSomethingBack;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!player.IsLanded)
            player.LandTrigger();

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) return;

        Vector3 relativePosition = transform.InverseTransformPoint(collision.GetContact(0).point);

        //if (relativePosition.x > 0)
        //{
        //    print("The object is to the right");
        //}
        //else
        //{
        //    print("The object is to the left");
        //}

        //if (relativePosition.y > 0)
        //{
        //    print("The object is above.");
        //}
        //else
        //{
        //    print("The object is below.");
        //}

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

        //Vector3 collisionPoint = collision.GetContact(0).normal;
        //Vector3 dir = collisionPoint

        //Debug.Log(collision.gameObject.name);

        if (collision.gameObject.TryGetComponent(out CollectableBase collectable))
        {
            collectable.Collect();
            player.PickUpTrigger(collectable.CollectableEffect);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CollectableBase collectable))
        {
            collectable.Collect();
            player.PickUpTrigger(collectable.CollectableEffect);
        }
    }
}
