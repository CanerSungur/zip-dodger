using UnityEngine;
using Cinemachine;
using System;
using DG.Tweening;

[RequireComponent(typeof(GameManager))]
public class CameraManager : MonoBehaviour
{
    private Player player;
    public Player Player { get { return player == null ? player = FindObjectOfType<Player>() : player; } }

    private GameManager gameManager;
    public GameManager GameManager { get { return gameManager == null ? gameManager = GetComponent<GameManager>() : gameManager; } }

    [Header("-- CAMERA SETUP --")]
    [SerializeField] private CinemachineFreeLook gameStartCM;
    [SerializeField] private CinemachineVirtualCamera gameplayCM;

    [Header("-- SHAKE SETUP --")]
    private CinemachineBasicMultiChannelPerlin gameplayCMBasicPerlin;
    private bool shakeStarted = false;
    private float shakeDuration = 1f;
    private float shakeTimer;

    [Header("-- POSITION SETUP --")]
    [SerializeField, Tooltip("Time that takes cam to update to its new Y axis upon zipper length change.")] private float camYAxisUpdateTime = 0.5f;
    [SerializeField, Tooltip("Amount that cam goes up or down to its new Y axis upon zipper length change.")] private float camYAxisChangeRate = 0.5f;
    [SerializeField, Tooltip("Amount of cam's FOV increase or decrease upon zipper length change.")] private float camFOVChangeRate = 1f;
    private CinemachineTransposer gameplayCMTransposer;
    private float defaultYOffset;
    private float defaultFOV;
    public float CurrentYOffset => gameplayCMTransposer.m_FollowOffset.y;
    public float CurrentFOV => gameplayCM.m_Lens.FieldOfView;

    public event Action OnShakeCam;

    private void Awake()
    {
        gameplayCMBasicPerlin = gameplayCM.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        gameplayCMBasicPerlin.m_AmplitudeGain = 0f;
        shakeTimer = shakeDuration;

        gameplayCMTransposer = gameplayCM.GetCinemachineComponent<CinemachineTransposer>();
        defaultYOffset = gameplayCMTransposer.m_FollowOffset.y;
        defaultFOV = gameplayCM.m_Lens.FieldOfView;

        gameStartCM.Priority = 1;
        gameplayCM.Priority = 0;
    }

    private void Start()
    {
        GameManager.OnGameStart += () => gameplayCM.Priority = 2;
        OnShakeCam += () => shakeStarted = true;

        Player.OnPickedUpSomething += UpdatePositon;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStart -= () => gameplayCM.Priority = 2;
        OnShakeCam -= () => shakeStarted = true;

        Player.OnPickedUpSomething -= UpdatePositon;

        transform.DOKill();
    }

    private void Update()
    {
        ShakeCamForAWhile();
    }

    private void UpdatePositon(CollectableEffect obj)
    {
        if (obj == CollectableEffect.SpawnZipper)
        {
            DOVirtual.Float(CurrentYOffset, defaultYOffset + (Player.CurrentRow * camYAxisChangeRate), camYAxisUpdateTime, newYOffset => {
                gameplayCMTransposer.m_FollowOffset.y = newYOffset;
            }).SetEase(Ease.InOutSine);

            DOVirtual.Float(CurrentFOV, defaultFOV + (Player.CurrentRow * camFOVChangeRate), camYAxisUpdateTime, newFOV => {
                gameplayCM.m_Lens.FieldOfView = newFOV;
            }).SetEase(Ease.InOutSine);
        }
    }

    private void ShakeCamForAWhile()
    {
        if (shakeStarted)
        {
            gameplayCMBasicPerlin.m_AmplitudeGain = 1f;

            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                shakeStarted = false;
                shakeTimer = shakeDuration;

                gameplayCMBasicPerlin.m_AmplitudeGain = 0f;
            }
        }
    }
}
