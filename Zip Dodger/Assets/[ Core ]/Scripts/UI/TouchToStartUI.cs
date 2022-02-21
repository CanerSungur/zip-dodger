using UnityEngine;
using UnityEngine.EventSystems;

public class TouchToStartUI : MonoBehaviour, IPointerDownHandler
{
    private UIManager uiManager;
    public UIManager UIManager { get { return uiManager == null ? uiManager = FindObjectOfType<UIManager>() : uiManager; } }

    public void OnPointerDown(PointerEventData eventData) => UIManager.StartGame();
}
