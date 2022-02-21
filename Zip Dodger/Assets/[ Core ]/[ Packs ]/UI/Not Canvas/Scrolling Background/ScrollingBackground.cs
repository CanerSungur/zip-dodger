using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private float x, y;

    private void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(x, y) * Time.deltaTime, image.uvRect.size);
    }
}
