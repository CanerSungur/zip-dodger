using UnityEngine;

/// <summary>
/// Holds all data about this game.
/// </summary>
[RequireComponent(typeof(GameManager))]
public class DataManager : MonoBehaviour
{
    private GameManager gameManager;
    public GameManager GameManager { get { return gameManager == null ? gameManager = GetComponent<GameManager>() : gameManager; } }

    public int TotalCoin { get; private set; }
    public int RewardCoin { get; private set; }
    public static int LevelEndMultiplier;

    private void Awake()
    {
        TotalCoin = PlayerPrefs.GetInt("TotalCoin", 0);
        RewardCoin = LevelEndMultiplier = 0;
    }

    private void Start()
    {
        GameManager.OnIncreaseCoin += IncreaseTotalCoin;
        GameManager.OnCalculateReward += CalculateReward;
    }

    private void OnDisable()
    {
        GameManager.OnIncreaseCoin -= IncreaseTotalCoin;
        GameManager.OnCalculateReward -= CalculateReward;
    }

    private void IncreaseTotalCoin(int amount)
    {
        TotalCoin += amount;
        PlayerPrefs.SetInt("TotalCoin", TotalCoin);
        PlayerPrefs.Save();
    }

    private void CalculateReward(int reward)
    {
        RewardCoin = reward;
        IncreaseTotalCoin(reward);
    }
}
