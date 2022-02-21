using UnityEngine;

[RequireComponent(typeof(Player))]
public class KeyboardInput : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        
    }
}
