using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsUI : MonoBehaviour
{
    //private LevelLoader levelLoader;
    //public LevelLoader LevelLoader { get { return levelLoader == null ? levelLoader = FindObjectOfType<LevelLoader>() : levelLoader; } }

    private UIManager uiManager;
    public UIManager UIManager { get { return uiManager == null ? uiManager = FindObjectOfType<UIManager>() : uiManager; } }

    public static bool IsSoundOn;
    public static bool IsVibrationOn;

    private GameObject vibrationObj;
    private GameObject soundObj;
    private GameObject replayObj;
    private GameObject replayConfirmationObj;

    private Image vibrationImage;
    private Image soundImage;

    private Animator backgroundAnim;
    private Animator menuAnim;
    private Animator vibrationAnim;
    private Animator soundAnim;
    private Animator replayAnim;

    private bool menuIsOpen = false;
    private bool vibrationOn = true;
    private bool soundOn = true;

    #region Animation Variables ID Setup

    private int openID = Animator.StringToHash("IsOpening");
    private int closeID = Animator.StringToHash("IsClosing");
    private int soundOnID = Animator.StringToHash("SoundOn");
    private int vibrationOnID = Animator.StringToHash("VibrationOn");

    #endregion

    private Color enabledColor = Color.white;
    private Color disabledColor = Color.gray;

    private void Awake()
    {
        IsSoundOn = true;
        IsVibrationOn = true;

        vibrationObj = transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
        soundObj = transform.GetChild(0).GetChild(1).GetChild(2).gameObject;
        replayObj = transform.GetChild(0).GetChild(1).GetChild(3).gameObject;
        replayConfirmationObj = transform.GetChild(0).GetChild(2).gameObject;

        vibrationImage = vibrationObj.GetComponent<Image>();
        soundImage = soundObj.GetComponent<Image>();

        backgroundAnim = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        menuAnim = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Animator>();
        vibrationAnim = vibrationObj.GetComponent<Animator>();
        soundAnim = soundObj.GetComponent<Animator>();
        replayAnim = replayObj.GetComponent<Animator>();

        enabledColor.a = 1f;
        disabledColor.a = .5f;

        replayConfirmationObj.SetActive(false);
    }

    #region Menu

    public void ToggleMenu()
    {
        if (menuIsOpen)
            CloseMenu();
        else
            OpenMenu();
    }

    private void OpenMenu()
    {
        backgroundAnim.SetBool(openID, true);
        menuAnim.SetBool(openID, true);
        vibrationAnim.SetBool(openID, true);
        soundAnim.SetBool(openID, true);
        replayAnim.SetBool(openID, true);

        backgroundAnim.SetBool(closeID, false);
        menuAnim.SetBool(closeID, false);
        vibrationAnim.SetBool(closeID, false);
        soundAnim.SetBool(closeID, false);
        replayAnim.SetBool(closeID, false);

        menuIsOpen = true;
    }

    private void CloseMenu()
    {
        backgroundAnim.SetBool(closeID, true);
        menuAnim.SetBool(closeID, true);
        vibrationAnim.SetBool(closeID, true);
        soundAnim.SetBool(closeID, true);
        replayAnim.SetBool(closeID, true);

        backgroundAnim.SetBool(openID, false);
        menuAnim.SetBool(openID, false);
        vibrationAnim.SetBool(openID, false);
        soundAnim.SetBool(openID, false);
        replayAnim.SetBool(openID, false);

        menuIsOpen = false;
    }

    #endregion

    #region Sound

    public void ToggleSound()
    {
        if (soundOn)
            CloseSound();
        else
            OpenSound();
    }

    private void OpenSound()
    {
        soundOn = true;
        soundImage.color = enabledColor;
        soundAnim.SetBool(soundOnID, true);

        IsSoundOn = true;
    }

    private void CloseSound()
    {
        soundOn = false;
        soundImage.color = disabledColor;
        soundAnim.SetBool(soundOnID, false);

        IsSoundOn = false;
    }

    #endregion

    #region Vibration

    public void ToggleVibration()
    {
        if (vibrationOn)
            CloseVibration();
        else
            OpenVibration();
    }

    private void OpenVibration()
    {
        vibrationOn = true;
        vibrationImage.color = enabledColor;
        vibrationAnim.SetBool(vibrationOnID, true);

        IsVibrationOn = true;
    }

    private void CloseVibration()
    {
        vibrationOn = false;
        vibrationImage.color = disabledColor;
        vibrationAnim.SetBool(vibrationOnID, false);

        IsVibrationOn = false;
    }

    #endregion

    public void Replay()
    {
        replayConfirmationObj.SetActive(true);
        CloseMenu();
        Time.timeScale = 0f;
    }

    #region Animation Event Functions

    public void ConfirmReplay()
    {
        Time.timeScale = 1f;
        UIManager.ChangeScene();
        Debug.Log("Replay Confirmed!");
    }
    public void DeclineReplay()
    {
        replayConfirmationObj.GetComponent<Animator>().SetTrigger(closeID);
        Debug.Log("Replay Canceled!");
    }
    public void CloseReplayConfirmationMenu()
    {
        replayConfirmationObj.SetActive(false);
        Time.timeScale = 1f;
    }

    #endregion
}
