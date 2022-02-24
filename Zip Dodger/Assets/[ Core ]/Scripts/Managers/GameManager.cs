using UnityEngine;
using System;

/// <summary>
/// Manages all the other managers. Holds game flow events.
/// If there will be a reward when level is finished, invoke OnCalculateReward instead of OnLevelSuccess.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Range(0.5f,5f)]public float GameSpeed = 1f;

    [Header("-- MANAGER REFERENCES --")]
    internal DataManager dataManager;
    internal UIManager uiManager;
    internal LevelManager levelManager;
    internal GameplayManager gameplayManager;

    [Header("-- END GAME PROPERTIES --")]
    [SerializeField, Tooltip("This coin will be multiplied with meter at the level end.")] int defaultRewardForLevelSuccess = 5;

    public int DefaultRewardForLevelSuccess => defaultRewardForLevelSuccess;
    public static GameState GameState { get; private set; }
    public static GameEnd GameEnd { get; private set; }

    public static event Action OnPlatformEnd;
    public event Action OnGameStart, OnGameEnd, OnLevelSuccess, OnLevelFail, OnChangeScene, OnChangePhase;
    public event Action<int> OnIncreaseCoin, OnCalculateReward;

    private void Awake()
    {
        TryGetComponent(out dataManager);
        TryGetComponent(out uiManager);
        TryGetComponent(out levelManager);
        TryGetComponent(out gameplayManager);

        ChangeState(GameState.WaitingToStart);
    }

    private void Start()
    {
        OnGameStart += () => ChangeState(GameState.Started);
        OnGameEnd += () => ChangeState(GameState.Finished);
        //OnPlatformEnd += () => ChangeState(GameState.PlatformIsOver);
        OnLevelSuccess += LevelSuccess;
        OnLevelFail += LevelFail;
    }

    private void OnDisable()
    {
        OnGameStart -= () => ChangeState(GameState.Started);
        OnGameEnd -= () => ChangeState(GameState.Finished);
        //OnPlatformEnd -= () => ChangeState(GameState.PlatformIsOver);
        OnLevelSuccess -= LevelSuccess;
        OnLevelFail -= LevelFail;

        OnPlatformEnd = OnGameStart = OnGameEnd = OnLevelSuccess = OnLevelFail = OnChangeScene = OnChangePhase = null;
        OnIncreaseCoin = OnCalculateReward = null;
    }

    private void Update()
    {
        Time.timeScale = GameSpeed;
    }

    private void LevelSuccess()
    {
        GameEnd = GameEnd.Win;
        AudioHandler.PlayAudio(AudioHandler.AudioType.Game_Win);
    }

    private void LevelFail()
    {
        GameEnd = GameEnd.Fail;
        AudioHandler.PlayAudio(AudioHandler.AudioType.Game_Fail);
    }

    private void ChangeState(GameState newState)
    {
        if (GameState != newState) GameState = newState;
    }

    public static void SetGameState(GameState state) => GameState = state;

    // Event Trigger Functions
    public void StartGameTrigger() => OnGameStart?.Invoke();
    public void EndGameTrigger() => OnGameEnd?.Invoke();
    public static void PlatformEndTrigger() => OnPlatformEnd?.Invoke();
    public void LevelSuccessTrigger() => OnLevelSuccess?.Invoke();
    public void LevelFailTrigger() => OnLevelFail?.Invoke();
    public void ChangeSceneTrigger() => OnChangeScene?.Invoke();
    public void CalculateRewardTrigger(int reward) => OnCalculateReward?.Invoke(reward);
    public void ChangePhaseTrigger() => OnChangePhase?.Invoke();
    public void IncreaseCoinTrigger(int amount) => OnIncreaseCoin?.Invoke(amount);

    public void InvokeEventDefault(Events eventName)
    {
        switch (eventName)
        {
            case Events.GameStart:
                OnGameStart?.Invoke();
                break;
            case Events.GameEnd:
                OnGameEnd?.Invoke();
                break;
            case Events.PlatformEnd:
                OnPlatformEnd?.Invoke();
                break;
            case Events.LevelSuccess:
                OnLevelSuccess?.Invoke();
                break;
            case Events.LevelFail:
                OnLevelFail?.Invoke();
                break;
            case Events.ChangeScene:
                OnChangeScene?.Invoke();
                break;
            case Events.ChangePhase:
                OnChangePhase?.Invoke();
                break;
            default:
                break;
        }
    }

    public void InvokeEventParamInt(Events_Param_Int eventName, int eventParam)
    {
        switch (eventName)
        {
            case Events_Param_Int.CalculateReward:
                OnCalculateReward?.Invoke(eventParam);
                break;
            case Events_Param_Int.IncreaseCoin:
                OnIncreaseCoin?.Invoke(eventParam);
                break;
            default:
                break;
        }
    }
}

public enum GameState
{
    WaitingToStart,
    Started,
    Paused,
    PlatformIsOver,
    Finished
}

public enum GameEnd { NotDecided, Fail, Win }

public enum Events
{
    GameStart,
    GameEnd,
    PlatformEnd,
    LevelSuccess,
    LevelFail,
    ChangeScene,
    ChangePhase
}

public enum Events_Param_Int
{
    CalculateReward,
    IncreaseCoin
}
