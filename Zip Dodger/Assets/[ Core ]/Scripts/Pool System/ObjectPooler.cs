using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    // Default is this object. You can serialize this to be another container object.
    private Transform poolContainer;

    //public PoolData PoolData;
    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    // Quick singleton to access easily.
    #region Singleton

    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
        poolContainer = this.transform;
    }

    #endregion

    private void Start()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.Size; i++)
            {
                // If PoolData has just 1 object;
                //GameObject obj = Instantiate(pool.PoolData.PoolObject, PoolContainer);

                // PoolData can have more than 1 object.
                //GameObject obj = Instantiate(pool.PoolData.PoolObjects[Random.Range(0, pool.PoolData.PoolObjects.Length)], PoolContainer);
                //GameObject obj = Instantiate(PoolData.PoolObjects[Random.Range(0, PoolData.PoolObjects.Length)].Prefab, PoolContainer);
                GameObject obj = Instantiate(pool.Prefab, poolContainer);

                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            PoolDictionary.Add(pool.Tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogError($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        // Pull out first element and store it
        GameObject objectToSpawn = PoolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // Add it back to our queue to use it later.
        PoolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
