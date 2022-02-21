using UnityEngine;
using DG.Tweening;

/// <summary>
/// Attach this script to whatever object you want it to bounce on enable.
/// </summary>
public class BounceOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DORewind();

        transform.DOShakePosition(.5f, .5f);
        transform.DOShakeRotation(.5f, .5f);
        transform.DOShakeScale(.5f, .5f);
    }
}
