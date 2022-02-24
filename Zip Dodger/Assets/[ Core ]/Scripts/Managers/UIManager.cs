using System;
using UnityEngine;
using ZestGames.Utility;

[RequireComponent(typeof(GameManager))]
public class UIManager : MonoBehaviour
{
    GameManager gameManager;
    public GameManager GameManager { get { return gameManager == null ? gameManager = GetComponent<GameManager>() : gameManager; } }

    [Header("-- UI REFERENCES --")]
    [SerializeField] TouchToStartUI touchToStart;
    [SerializeField] HUDUI hud;
    [SerializeField] LevelSuccessUI levelSuccess;
    [SerializeField] LevelFailUI levelFail;
    [SerializeField] SettingsUI settings;

    [Header("-- UI DELAY SETUP --")]
    [SerializeField, Tooltip("The delay in seconds between the game is won and the win screen is loaded.")] float winScreenDelay = 3.0f;
    [SerializeField, Tooltip("The delay in secods between the game is lost and the fail screen is loaded.")] float failScreenDelay = 3.0f;

    public Transform CoinHUDTransform => hud.CoinHUDTransform;

    void Awake()
    {
        Init();
    }

    void Start()
    {
        GameManager.OnGameStart += GameStarted;
        GameManager.OnGameEnd += GameEnded;
        GameManager.OnPlatformEnd += PlatformEnded;
        GameManager.OnLevelSuccess += LevelSuccess;
        GameManager.OnLevelFail += LevelFail;

        GameManager.OnIncreaseCoin += hud.UpdateCoinUITrigger;
    }

    void PlatformEnded() => settings.gameObject.SetActive(false);

    void OnDisable()
    {
        GameManager.OnGameStart -= GameStarted;
        GameManager.OnGameEnd -= GameEnded;
        GameManager.OnPlatformEnd -= PlatformEnded;
        GameManager.OnLevelSuccess -= LevelSuccess;
        GameManager.OnLevelFail -= LevelFail;

        GameManager.OnIncreaseCoin -= hud.UpdateCoinUITrigger;
    }

    void Init()
    {
        touchToStart.gameObject.SetActive(true);

        settings.gameObject.SetActive(false);
        hud.gameObject.SetActive(false);
        levelSuccess.gameObject.SetActive(false);
        levelFail.gameObject.SetActive(false);
    }

    void GameStarted()
    {
        settings.gameObject.SetActive(true);
        hud.gameObject.SetActive(true);
        hud.UpdateCoinUITrigger(GameManager.dataManager.TotalCoin);
        hud.UpdateLevelUTrigger(GameManager.levelManager.Level);

        touchToStart.gameObject.SetActive(false);
    }

    void GameEnded()
    {
        settings.gameObject.SetActive(false);
        hud.gameObject.SetActive(false);
    }
    void LevelSuccess()
    {
        Utils.DoActionAfterDelay(this, winScreenDelay, () => levelSuccess.gameObject.SetActive(true));
        Utils.DoActionAfterDelay(this, winScreenDelay, () => hud.gameObject.SetActive(false));
    }
        
    void LevelFail()
    {
        Utils.DoActionAfterDelay(this, failScreenDelay, () => levelFail.gameObject.SetActive(true));
        Utils.DoActionAfterDelay(this, failScreenDelay, () => hud.gameObject.SetActive(false));
    }

    // Functions for dependant classes
    public void StartGame() => GameManager.StartGameTrigger();
    public void ChangeScene() => GameManager.ChangeSceneTrigger();
}
