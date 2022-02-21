using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerZipperSpawner : MonoBehaviour
{
    private Player player;
    public Player Player { get { return player == null ? player = GetComponent<Player>() : player; } }

    [Header("-- SETUP --")]
    [SerializeField] private GameObject childZipperPrefab;

    private void Start()
    {
        Player.OnPickedUpSomething += HandlePickUp;
    }

    private void OnDisable()
    {
        Player.OnPickedUpSomething -= HandlePickUp;
    }

    private void HandlePickUp(CollectableEffect obj)
    {
        if (obj != CollectableEffect.SpawnZipper) return;

        Player.CurrentRow++;
        //ChildZipper zipper = Instantiate(childZipperPrefab, transform.position + (Vector3.forward * (Player.CurrentRow * 1.5f)), Quaternion.identity).GetComponent<ChildZipper>();
        ChildZipper zipper = Instantiate(childZipperPrefab, transform.position + new Vector3(Player.CurrentRow * 1.5f, 0f, Random.Range(-2f, 2f)), Quaternion.identity).GetComponent<ChildZipper>();
        
        Player.ChildZippers.Add(zipper);

        zipper.SetRow(Player.CurrentRow);
        zipper.SetFollowTarget();
    }
}
