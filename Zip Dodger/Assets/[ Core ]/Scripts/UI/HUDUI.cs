using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class HUDUI : MonoBehaviour
{
    private UIManager uiManager;
    public UIManager UIManager { get { return uiManager == null ? uiManager = FindObjectOfType<UIManager>() : uiManager; } }

    private Animator animator;
    public Animator Animator { get { return animator == null ? animator = GetComponent<Animator>() : animator; } }

    [Header("-- TEXT REFERENCES --")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("-- COIN SETUP --")]
    [SerializeField] private Transform coinHUDTransform;
    public Transform CoinHUDTransform => coinHUDTransform;

    public event Action<int> OnUpdateCoinUI;
    public event Action<int> OnUpdateLevelUI;

    private void OnEnable()
    {
        Animator.enabled = false;

        OnUpdateCoinUI += UpdateCoinText;
        OnUpdateLevelUI += UpdateLevelText;
    }

    private void OnDisable()
    {
        OnUpdateCoinUI -= UpdateCoinText;
        OnUpdateLevelUI -= UpdateLevelText;
    }

    public void UpdateCoinUITrigger(int ignoreThis) => OnUpdateCoinUI?.Invoke(ignoreThis);
    public void UpdateLevelUTrigger(int level) => OnUpdateLevelUI?.Invoke(level);
    private void UpdateLevelText(int level)
    {
        //Debug.Log("Updated Coin Text");
        levelText.text = $"Level {level}";
    }
    private void UpdateCoinText(int ignoreThis)
    {
        //Debug.Log("Updated Level Text");
        //coinText.text = coin.ToString();
        coinText.text = UIManager.GameManager.dataManager.TotalCoin.ToString();

        ShakeCoinHUD();
    }

    private void ShakeCoinHUD()
    {
        CoinHUDTransform.DORewind();

        CoinHUDTransform.DOShakePosition(.5f, .5f);
        CoinHUDTransform.DOShakeRotation(.5f, .5f);
        CoinHUDTransform.DOShakeScale(.5f, .5f);
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
