using UnityEngine;
using TMPro;

public class LevelSuccessUI : MonoBehaviour
{
    private UIManager uiManager;
    public UIManager UIManager { get { return uiManager == null ? uiManager = FindObjectOfType<UIManager>() : uiManager; } }

    private TextMeshProUGUI levelText;
    TextMeshProUGUI multiplierText;
    TextMeshProUGUI scoreText;

    private CustomButton nextButton;

    private void OnEnable()
    {
        levelText = transform.GetChild(transform.childCount - 1).GetComponentInChildren<TextMeshProUGUI>();
        levelText.text = "Level " + (UIManager.GameManager.levelManager.Level - 1); // -1 because level is increased immediately on level success.

        multiplierText = transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
        multiplierText.text = "x" + DataManager.LevelEndMultiplier;

        scoreText= transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        scoreText.text = UIManager.GameManager.dataManager.RewardCoin.ToString();

        nextButton = GetComponentInChildren<CustomButton>();
        nextButton.onClick.AddListener(NextButtonClicked);
    }

    private void OnDisable()
    {
        //nextButton.onClick.RemoveListener(NextButtonClicked);

        nextButton.onClick.RemoveListener(NextButtonClicked);
    }

    private void NextButtonClicked() => nextButton.ClickTrigger(UIManager.ChangeScene);

    //private void NextButtonClicked()
    //{
    //    UIManager.ChangeScene();
    //}
}
