using UnityEngine;

public class PlatformEnd : MonoBehaviour
{
    private GameManager gameManager;
    public GameManager GameManager => gameManager == null ? gameManager = FindObjectOfType<GameManager>() : gameManager;

    [Header("-- SETUP --")]
    [SerializeField] Color[] colors;
    [SerializeField] PlatformEndMeter[] meters;
    float activatedYAxis = 0.7f;
    float defaultYAxis = 0f;
    int distanceBetweenMeters = 4;

    public Color[] Colors => colors;
    public float ActivatedYAxis => activatedYAxis;
    public float DefaultYAxis => defaultYAxis;
    public int DistanceBetweenMeters => distanceBetweenMeters;

    private void Awake()
    {
        InitMeters();   
    }

    private void InitMeters()
    {
        for (int i = 0; i < meters.Length; i++)
            meters[i].SetIndex(i + 1);
    }
}
