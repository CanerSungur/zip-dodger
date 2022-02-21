using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam;

    private void OnEnable()
    {
        cam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
