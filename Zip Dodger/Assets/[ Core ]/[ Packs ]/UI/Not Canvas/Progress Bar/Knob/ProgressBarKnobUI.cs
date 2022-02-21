using UnityEngine;

public class ProgressBarKnobUI : MonoBehaviour
{
    private HUDUI hudUI;
    public HUDUI HUDUI { get { return hudUI == null ? hudUI = GetComponentInParent<HUDUI>() : hudUI; } }

    [Header("-- KNOB REFERENCES --")]
    [SerializeField] private Animation phase_01_Finished_Anim;
    [SerializeField] private Animation phase_02_Finished_Anim;
    [SerializeField] private Animation phase_03_Finished_Anim;

    [Header("-- LINE REFERENCES --")]
    [SerializeField] private Animation line_01_Anim;
    [SerializeField] private Animation line_02_Anim;

    private void OnEnable()
    {
        if (!HUDUI.UIManager.GameManager) return;
        HUDUI.UIManager.GameManager.OnChangePhase += EnableRelevantKnob;
    }

    private void OnDisable()
    {
        if (!HUDUI.UIManager.GameManager) return;
        HUDUI.UIManager.GameManager.OnChangePhase -= EnableRelevantKnob;
    }

    private void EnableRelevantKnob()
    {
        //if (HUDUI.UIManager.GameManager.gameplayManager.PhaseIsFinished) return;

        switch (HUDUI.UIManager.GameManager.gameplayManager.CurrentPhaseCount)
        {
            case 2:
                phase_01_Finished_Anim.Play();
                break;
            case 3:
                phase_02_Finished_Anim.Play();
                line_01_Anim.Play();
                break;
            case 4:
                phase_03_Finished_Anim.Play();
                line_02_Anim.Play();
                break;
        }
    }
}
