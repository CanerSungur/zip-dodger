using UnityEngine;
using System;

[RequireComponent(typeof(GameManager))]
public class CollectableManager : MonoBehaviour
{
    private GameManager gameManager;
    public GameManager GameManager { get { return gameManager == null ? gameManager = GetComponent<GameManager>() : gameManager; } }

    [Header("-- SETUP --")]
    [SerializeField, Tooltip("Coin prefab that will be instantiated upon scene starts.")] private GameObject coinPrefab;
    [SerializeField, Tooltip("Trap prefab that will be instantiated upon scene starts.")] private GameObject trapPrefab;
    [SerializeField, Tooltip("Amount of coin that will be instantiated.")] private int coinAmountInScene;
    [SerializeField, Tooltip("Amount of trap that will be instantiated.")] private int trapAmountInScene;
    [SerializeField, Tooltip("Object that will be spawned as reward when an object is destroyed.")] private GameObject coinRewardPrefab;
    [SerializeField, Tooltip("Offset relative to the destroyed object's position.")] private float spawnPointOffset = 2.75f;
    public Transform CoinHUDTransform => GameManager.uiManager.CoinHUDTransform;

    public static event Action<Vector3, int> OnSpawnCoinRewards;

    private void Start()
    {
        OnSpawnCoinRewards += SpawnCoinRewards;
    }

    private void OnDisable()
    {
        OnSpawnCoinRewards -= SpawnCoinRewards;
    }

    //private void InitCollectables()
    //{
    //    ObjectPooler.Instance.SpawnFromPool("Coin", );
    //}

    private void SpawnCoinRewards(Vector3 spawnPosition, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(coinRewardPrefab,spawnPosition + (Vector3.up * spawnPointOffset), Quaternion.identity);
        }
    }

    public static void SpawnCoinRewardsTrigger(Vector3 spawnPosition, int amount) => OnSpawnCoinRewards?.Invoke(spawnPosition, amount);
}
