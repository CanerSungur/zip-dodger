using UnityEngine;

[RequireComponent(typeof(AI))]
public class AIAudio : MonoBehaviour
{
    private AI ai;

    private void Awake()
    {
        ai = GetComponent<AI>();
    }

    //private void Start()
    //{
    //    player.OnJump += Jump;
    //    player.OnLand += Land;
    //}

    //private void OnDisable()
    //{
    //    player.OnJump -= Jump;
    //    player.OnLand -= Land;
    //}

    //private void Jump() => AudioHandler.PlayAudio(AudioHandler.AudioType.Player_Jump, transform.position);
    //private void Land() => AudioHandler.PlayAudio(AudioHandler.AudioType.Player_Land, transform.position);
}
