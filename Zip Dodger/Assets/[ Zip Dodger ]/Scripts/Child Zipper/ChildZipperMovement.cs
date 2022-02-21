using UnityEngine;

[RequireComponent(typeof(ChildZipper))]
public class ChildZipperMovement : MonoBehaviour
{
    private ChildZipper childZipper;
    public ChildZipper ChildZipper { get { return childZipper == null ? childZipper = GetComponent<ChildZipper>() : childZipper; } }

    private void FixedUpdate()
    {
        if (ChildZipper.IsDetached) return;

        Vector3 newPos = ChildZipper.FollowTarget.position + (Vector3.forward * ChildZipper.FollowDistance);
        transform.position = Vector3.Lerp(transform.position, newPos, ChildZipper.Speed * Time.fixedDeltaTime);
    }
}
