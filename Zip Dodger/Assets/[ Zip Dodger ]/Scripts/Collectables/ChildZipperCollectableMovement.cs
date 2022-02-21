using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(ChildZipperCollectable))]
public class ChildZipperCollectableMovement : MonoBehaviour
{
    private ChildZipperCollectable childZipperCollectable;
    public ChildZipperCollectable ChildZipperCollectable { get { return childZipperCollectable == null ? childZipperCollectable = GetComponent<ChildZipperCollectable>() : childZipperCollectable; } }

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
        transform.DOMove(new Vector3(transform.position.x + -0.5f, transform.position.y, transform.position.z + 0.5f), ChildZipperCollectable.CycleLength).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        transform.DORotate(new Vector3(360, 0, 180), ChildZipperCollectable.CycleLength, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
}
