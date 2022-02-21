using UnityEngine;

public class SettingsUIAnimationListener : MonoBehaviour
{
    public enum ButtonType { ConfirmButton, DeclineButton, Menu }
    public ButtonType Type;
    private SettingsUI settingsMenu;
    public SettingsUI SettingsMenu { get { return settingsMenu == null ? settingsMenu = GetComponentInParent<SettingsUI>() : settingsMenu; } }

    public void AlertObservers()
    {
        if (Type == ButtonType.ConfirmButton)
            Confirm();
        else if (Type == ButtonType.DeclineButton)
            Decline();
        else if (Type == ButtonType.Menu)
            Disable();
    }
    private void Confirm() => SettingsMenu.ConfirmReplay();
    private void Decline() => SettingsMenu.DeclineReplay();
    private void Disable() => SettingsMenu.CloseReplayConfirmationMenu();
    public void PlaySound() => AudioHandler.PlayAudio(AudioHandler.AudioType.Button_Click);
}
