using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class HUDUI : MonoBehaviour
{
    UIManager uiManager;
    public UIManager UIManager { get { return uiManager == null ? uiManager = FindObjectOfType<UIManager>() : uiManager; } }

    Animator animator;
    public Animator Animator { get { return animator == null ? animator = GetComponent<Animator>() : animator; } }

    [Header("-- TEXT REFERENCES --")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("-- COIN SETUP --")]
    [SerializeField] private Transform coinHUDTransform;
    public Transform CoinHUDTransform => coinHUDTransform;

    [Header("-- PROGRESS SETUP --")]
    [SerializeField] GameObject progressHorizontal;
    [SerializeField] GameObject progressKnob;

    public event Action<int> OnUpdateCoinUI;
    public event Action<int> OnUpdateLevelUI;

    void OnEnable()
    {
        Animator.enabled = false;

        OnUpdateCoinUI += UpdateCoinText;
        OnUpdateLevelUI += UpdateLevelText;

        GameManager.OnPlatformEnd += CloseProgressBar;
    }

    void OnDisable()
    {
        OnUpdateCoinUI -= UpdateCoinText;
        OnUpdateLevelUI -= UpdateLevelText;

        GameManager.OnPlatformEnd -= CloseProgressBar;
    }

    public void UpdateCoinUITrigger(int ignoreThis) => OnUpdateCoinUI?.Invoke(ignoreThis);
    public void UpdateLevelUTrigger(int level) => OnUpdateLevelUI?.Invoke(level);
    void UpdateLevelText(int level)
    {
        //Debug.Log("Updated Coin Text");
        levelText.text = $"Level {level}";
    }
    void UpdateCoinText(int ignoreThis)
    {
        //Debug.Log("Updated Level Text");
        //coinText.text = coin.ToString();
        coinText.text = UIManager.GameManager.dataManager.TotalCoin.ToString();

        ShakeCoinHUD();
    }

    void ShakeCoinHUD()
    {
        CoinHUDTransform.DORewind();

        CoinHUDTransform.DOShakePosition(.5f, .5f);
        CoinHUDTransform.DOShakeRotation(.5f, .5f);
        CoinHUDTransform.DOShakeScale(.5f, .5f);
    }

    public void CloseProgressBar()
    {
        if (progressHorizontal)
            progressHorizontal.SetActive(false);
        else if (progressKnob)
            progressKnob.SetActive(false);
        else
            Debug.Log("No Progress Bar.");
    }

    // Animation event listener.
    public void AlertObservers(string message)
    {
        if (message.Equals("RewardAnimEnded")) // Level success screen should trigger here.
            UIManager.GameManager.LevelSuccessTrigger();
        else if (message.Equals("UpdateCoinAfterReward"))
            UpdateCoinUITrigger(UIManager.GameManager.dataManager.TotalCoin);
    }
}
