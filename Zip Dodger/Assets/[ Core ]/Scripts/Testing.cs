using UnityEngine;

public class Testing : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Vector3 spawnPos = new Vector3(Random.Range(-15f, 15f), Random.Range(5f, 15f), Random.Range(-10f,10f));
            ObjectPooler.Instance.SpawnFromPool("Stand", spawnPos, Quaternion.identity);
        }
    }
}
