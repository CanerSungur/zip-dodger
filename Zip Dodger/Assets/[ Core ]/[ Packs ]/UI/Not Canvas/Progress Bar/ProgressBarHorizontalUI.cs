using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Use either UpdateProgressBar in Update or SmoothUpdateProgressBar when something happens.
/// </summary>
public class ProgressBarHorizontalUI : MonoBehaviour
{
    [Header("-- REFERENCES --")]
    [SerializeField, Tooltip("Considered as start point or focused object through progression i.e. Player.")] private Transform focusTransform;
    [SerializeField, Tooltip("Considered as end point i.e. Finish Line.")] private Transform finishTransform;
    [SerializeField, Tooltip("Custom gradient if you're not using images for filling. Leave it white when using images!")] private Gradient gradient;
    [SerializeField, Tooltip("Seconds that will take to update the progress bar.")] private float updateSpeedSeconds = 0.5f;

    [Header("-- SETUP --")]
    private Slider slider;
    private Image fill;
    private float currentZPosition = 0;
    private float finishZPosition;

    private void Awake()
    {
        // That's because game is usually progressing on z axis.
        finishZPosition = finishTransform.position.z;
        currentZPosition = focusTransform.position.z;

        slider = GetComponent<Slider>();
        fill = transform.GetChild(1).GetChild(0).GetComponent<Image>();

        fill.color = gradient.Evaluate(1f);
        slider.maxValue = finishZPosition;
        slider.minValue = currentZPosition;

        slider.value = currentZPosition;
    }

    private void Update()
    {
        UpdateProgressBar();
    }

    private void CheckIfFocusPassedFinish()
    {
        if (focusTransform.position.z > currentZPosition)
            currentZPosition = focusTransform.position.z;
    }

    private void UpdateProgressBar()
    {
        CheckIfFocusPassedFinish();

        slider.value = currentZPosition;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    private IEnumerator SmoothUpdateProgressBar()
    {
        CheckIfFocusPassedFinish();

        float preChange = slider.value;
        float elapsed = 0f;
        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            Mathf.Lerp(preChange, currentZPosition, 0);

            slider.value = Mathf.Lerp(preChange, currentZPosition, elapsed / updateSpeedSeconds);
            yield return null;
        }

        slider.value = currentZPosition;
    }
}
