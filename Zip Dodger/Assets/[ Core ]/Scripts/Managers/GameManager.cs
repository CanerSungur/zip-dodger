using UnityEngine;
using System;

/// <summary>
/// Manages all the other managers. Holds game flow events.
/// If there will be a reward when level is finished, invoke OnCalculateReward instead of OnLevelSuccess.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("-- MANAGER REFERENCES --")]
    internal DataManager dataManager;
    internal UIManager uiManager;
    internal LevelManager levelManager;
    internal GameplayManager gameplayManager;

    public static GameState GameState { get; private set; }
    public static GameEnd GameEnd { get; private set; }

    public event Action OnGameStart, OnGameEnd, OnLevelSuccess, OnLevelFail, OnChangeScene, OnCalculateReward, OnChangePhase;
    public event Action<int> OnIncreaseCoin;

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
        OnLevelSuccess += LevelSuccess;
        OnLevelFail += LevelFail;
    }

    private void OnDisable()
    {
        OnGameStart -= () => ChangeState(GameState.Started);
        OnGameEnd -= () => ChangeState(GameState.Finished);
        OnLevelSuccess -= LevelSuccess;
        OnLevelFail -= LevelFail;
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

    // Event Trigger Functions
    public void StartGameTrigger() => OnGameStart?.Invoke();
    public void EndGameTrigger() => OnGameEnd?.Invoke();
    public void LevelSuccessTrigger() => OnLevelSuccess?.Invoke();
    public void LevelFailTrigger() => OnLevelFail?.Invoke();
    public void ChangeSceneTrigger() => OnChangeScene?.Invoke();
    public void CalculateRewardTrigger() => OnCalculateReward?.Invoke();
    public void ChangePhaseTrigger() => OnChangePhase?.Invoke();
    public void IncreaseCoinTrigger(int amount) => OnIncreaseCoin?.Invoke(amount);
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
