using UnityEngine;

[RequireComponent(typeof(AI))]
public class AIAnimationController : MonoBehaviour
{
    private GameManager gameManager;
    public GameManager GameManager { get { return gameManager == null ? gameManager = FindObjectOfType<GameManager>() : gameManager; } }

    private AI ai;

    [Header("-- ANIMATION NAME SETUP --")]
    private readonly int onGroundID = Animator.StringToHash("OnGround");
    private readonly int runID = Animator.StringToHash("Run");
    private readonly int dieID = Animator.StringToHash("Die");
    private readonly int winID = Animator.StringToHash("Win");
    private readonly int failID = Animator.StringToHash("Fail");

    private void Awake()
    {
        ai = GetComponent<AI>();
    }

    private void Start()
    {
        if (GameManager)
            GameManager.OnGameEnd += WinOrFail;

        Idle();
    }

    private void OnDisable()
    {
        if (GameManager)
            GameManager.OnGameEnd -= WinOrFail;
    }

    private void Update()
    {
        if (GameManager.GameState == GameState.Finished) return;

        if (ai.IsDead)
        {
            Die();
            return;
        }

        if (ai.IsMoving)
            Run();
        else
            Idle();
    }

    private void Idle() => ai.animator.SetBool(runID, false);

    private void Run() => ai.animator.SetBool(runID, true);
    private void Die() => ai.animator.SetTrigger(dieID);

    private void WinOrFail()
    {
        if (ai._Side == AI.Side.Ally)
            ai.animator.SetTrigger(winID);
        else if (ai._Side == AI.Side.Enemy)
            ai.animator.SetTrigger(failID);
        else
            Debug.Log("No Winners...");
    }
}
