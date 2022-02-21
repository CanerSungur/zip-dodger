using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(Trap))]
public class TrapMovement : MonoBehaviour
{
    private Trap trap;
    public Trap Trap { get { return trap == null ? trap = GetComponent<Trap>() : trap; } }

    public event Action OnStartMovement;

    private void OnEnable()
    {
        OnStartMovement += StartMoving;
    }

    private void OnDisable()
    {
        OnStartMovement -= StartMoving;

        transform.DOKill();
    }

    public void StartMovingTrigger() => OnStartMovement?.Invoke();
    private void StartMoving()
    {
        transform.DOMove(new Vector3(transform.position.x + -0.5f, transform.position.y, transform.position.z + 0.5f), Trap.CycleLength).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        transform.DORotate(new Vector3(360, 0, 180), Trap.CycleLength, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
}
