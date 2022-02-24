using UnityEngine;

public class ShortPlatformTrigger : MonoBehaviour
{
    private Player player;
    public Player Player => player == null ? player = FindObjectOfType<Player>() : player;

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ZipperPairHandler zipper) && !isTriggered)
        {
            if (zipper._Type == ZipperPairHandler.Type.Child && !Player.IsOnShortPlatform)
            {
                Player.IsOnShortPlatform = true;
                isTriggered = true;
            }
            else if (zipper._Type == ZipperPairHandler.Type.Parent && Player.IsOnShortPlatform)
            {
                Player.IsOnShortPlatform = false;
                isTriggered = true;
            }
        }
    }
}
