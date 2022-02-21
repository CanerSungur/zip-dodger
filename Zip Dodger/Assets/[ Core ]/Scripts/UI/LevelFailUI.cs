using UnityEngine;
using UnityEngine.UI;
using ZestGames.Vibrate;

public class LevelFailUI : MonoBehaviour
{
    private UIManager uiManager;
    public UIManager UIManager { get { return uiManager == null ? uiManager = FindObjectOfType<UIManager>() : uiManager; } }

    private CustomButton restartButton;

    private void OnEnable()
    {
        if (Vibration.HasVibrator())
            Vibration.VibratePredefined(1, true);

        restartButton = GetComponentInChildren<CustomButton>();
        restartButton.onClick.AddListener(RestartButtonClicked);
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveListener(RestartButtonClicked);
    }

    private void RestartButtonClicked() => restartButton.ClickTrigger(UIManager.ChangeScene);
}
