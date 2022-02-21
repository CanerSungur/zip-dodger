using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(Coin))]
public class CoinMovement : MonoBehaviour
{
    private Coin coin;
    public Coin Coin { get { return coin == null ? coin = GetComponent<Coin>() : coin; } }

    private bool triggerMoving = false;
    private bool coinHasReached = false;
    private bool startedRotating = false;
    private Vector3 coinHUDWorldPos;
    private float distanceBetweenCamAndPlayer;
    private Camera cam;

    public event Action OnStartMovement;

    private void OnEnable()
    {
        cam = Camera.main;

        if (Coin.CollectableStyle == CollectableStyle.Reward)
        {
            transform.localScale = Vector3.zero;

            transform.DOScale(1, 1f).SetEase(Ease.OutElastic);
            transform.DOMove(new Vector3(UnityEngine.Random.Range(transform.position.x - .75f, transform.position.x + .75f), UnityEngine.Random.Range(transform.position.y, transform.position.y + 1f), UnityEngine.Random.Range(transform.position.z - .5f, transform.position.z + .5f)), 0.5f).SetEase(Ease.InOutSine).OnComplete(() => {
                transform.DOMove(transform.position, 0.5f).OnComplete(() => triggerMoving = true);
            });
        }

        OnStartMovement += StartMoving;
    }

    private void OnDisable()
    {
        OnStartMovement -= StartMoving;

        transform.DOKill();
    }

    private void Update()
    {
        if (triggerMoving)
        {
            distanceBetweenCamAndPlayer = (CharacterPositionHolder.PlayerInScene.transform.position - cam.transform.position).magnitude;
            coinHUDWorldPos = cam.ScreenToWorldPoint(Coin.CollectableManager.CoinHUDTransform.position + new Vector3(-0.5f, 0f, distanceBetweenCamAndPlayer));
            Vector3 dir = coinHUDWorldPos - transform.position;
            transform.Translate(dir * Coin.MovementSpeed * Time.deltaTime, Space.World);
            //transform.DOMove(coinHUDWorldPos, Coin.MovementTime).SetEase(Ease.InSine);
            if (!startedRotating)
            {
                transform.DORotate(new Vector3(0, 360, 0), Coin.MovementSpeed * 0.25f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
                startedRotating = true;
            }
            //triggerMoving = false;
        }

        if (Vector3.Distance(transform.position, coinHUDWorldPos) <= 1f && !coinHasReached)
        {
            triggerMoving = false;
            //transform.DOScale(new Vector3(0.5f, 1.2f, 0.7f), 0.5f).SetEase(Ease.InElastic).OnComplete(() => {
            //    transform.DOScale(new Vector3(1.1f, 0.7f, 1.2f), 0.5f).SetEase(Ease.InElastic);
            //});
            //PlayerStats.OnIncreaseCoin?.Invoke(1);
            
            //HUDUI.UpdateCoinTrigger(value);
            Coin.CollectableManager.GameManager.IncreaseCoinTrigger(Coin.Value);
            transform.DOScale(0, 0.5f).SetEase(Ease.OutElastic).OnComplete(() => {


                transform.DOKill();
                Destroy(gameObject, 1f);
            });
            coinHasReached = true;
        }
    }

    public void StartMovingTrigger() => OnStartMovement?.Invoke();
    private void StartMoving() => triggerMoving = true;
}
