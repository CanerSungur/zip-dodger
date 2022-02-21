using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZestGames.AIBrain;

[RequireComponent(typeof(AI))]
public class AIAction : MonoBehaviour
{
    private AI ai;

    [Header("-- ACTIONS SETUP --")]
    [SerializeField, Tooltip("Delay in seconds for ai to check another closest target.")] private float updateTargetDelay = 3f;
    private List<Transform> enemies = new List<Transform>();
    private Transform target;

    public Transform Target => target;

    private void Awake()
    {
        ai = GetComponent<AI>();
    }

    private void Start()
    {
        StartCoroutine(UpdateTargetWithDelay(updateTargetDelay));
    }

    private IEnumerator UpdateTargetWithDelay(float delay)
    {
        while (!ai.IsDead)
        {
            UpdateEnemyList();
            target = AIBrain.FindClosestTransform(transform, enemies);
            Debug.Log($"{name}'s target: {target.name}");

            yield return new WaitForSeconds(delay);
        }
    }

    private void UpdateEnemyList()
    {
        enemies.Clear();

        for (int i = 0; i < CharacterPositionHolder.AIsInScene.Count; i++)
        {
            if (!enemies.Contains(CharacterPositionHolder.AIsInScene[i].transform))
            enemies.Add(CharacterPositionHolder.AIsInScene[i].transform);
        }

        enemies.Add(CharacterPositionHolder.PlayerInScene.transform);
    }
}
