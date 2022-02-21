using UnityEngine;

[RequireComponent(typeof(Player))]
public class RunnerForwardMover : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        if (!player.IsControllable) return;

        //player.rb.velocity = Vector3.forward * player.CurrentMovementSpeed;
        player.rb.MovePosition(Vector3.forward * player.CurrentMovementSpeed * Time.fixedDeltaTime);
    }
}
