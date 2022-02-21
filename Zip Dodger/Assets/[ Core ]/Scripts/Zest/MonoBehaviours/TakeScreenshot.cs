using UnityEngine;
using ZestGames.ScreenshotSystem;

/// <summary>
/// Attach this to a game object on scene to start taking screenshot by pressing 'Space' key.
/// </summary>
public class TakeScreenshot : MonoBehaviour
{
    private void Update()
    {
        Screenshot.TakeAScreenshot();
    }
}
