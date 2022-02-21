using UnityEngine;

[RequireComponent(typeof(AI))]
public class AINavMeshMovement : MonoBehaviour
{
    private AI ai;
    private Transform currentTarget;

    private void Awake()
    {
        ai = GetComponent<AI>();
    }

    private void Update()
    {
        if (!ai.CanMove) return;

        ai.navMeshAgent.SetDestination(ai.aiAction.Target.position);
        ai.navMeshAgent.speed = ai.CurrentMovementSpeed;
    }
}
