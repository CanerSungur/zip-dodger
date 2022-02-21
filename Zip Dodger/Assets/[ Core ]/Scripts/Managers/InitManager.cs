using UnityEngine;

public class InitManager : MonoBehaviour
{
    private void Start()
    {
        GetComponent<GameManager>().ChangeSceneTrigger();
    }
}
