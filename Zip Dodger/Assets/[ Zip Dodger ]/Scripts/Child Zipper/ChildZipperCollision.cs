using UnityEngine;

[RequireComponent(typeof(ChildZipper))]
public class ChildZipperCollision : MonoBehaviour
{
    private ChildZipper childZipper;
    public ChildZipper ChildZipper { get { return childZipper == null ? childZipper = GetComponent<ChildZipper>() : childZipper; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CollectableBase collectable))
        {
            collectable.Collect();
            ChildZipper.PickUpTrigger(collectable.CollectableEffect);
        }
    }
}
