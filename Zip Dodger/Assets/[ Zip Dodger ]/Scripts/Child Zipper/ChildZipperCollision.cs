using UnityEngine;

[RequireComponent(typeof(ChildZipper))]
public class ChildZipperCollision : MonoBehaviour
{
    private ChildZipper childZipper;
    public ChildZipper ChildZipper { get { return childZipper == null ? childZipper = GetComponent<ChildZipper>() : childZipper; } }

    private void OnCollisionEnter(Collision collision)
    {
        if (!ChildZipper.IsDetached && collision.gameObject.TryGetComponent(out Grinder grinder))
        {
            grinder.Execute();
            ChildZipper.Player.DetachZipperTrigger(ChildZipper.Row);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!ChildZipper.IsDetached && other.TryGetComponent(out CollectableBase collectable))
        {
            collectable.Collect();
            ChildZipper.PickUpTrigger(collectable.CollectableEffect);
        }
    }
}
