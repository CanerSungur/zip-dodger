using UnityEngine;

public class Gap : MonoBehaviour
{
    private Player player;
    public Player Player => player == null ? player = FindObjectOfType<Player>() : player; 

    [Header("-- REFERENCES --")]
    internal GapInput input;
    internal GapMovement movement;

    [Header("-- MOVEMENT SETUP --")]
    [SerializeField] private float speed = 5f;

    [Header("-- INPUT SETUP --")]
    [SerializeField] private float swerveSpeed = 0.5f;
    [SerializeField] private float maxSwerveAmount = 1f;

    public float Speed => speed;
    public float SwerveSpeed => swerveSpeed;
    public float MaxSwerveAmount => maxSwerveAmount;
    public bool IsControllable => GameManager.GameState == GameState.Started;

    private void Awake()
    {
        TryGetComponent(out movement);
        TryGetComponent(out input);
    }
}
