using UnityEngine;

public class ZipperPairHandler : MonoBehaviour
{
    public enum Side { Left, Right }
    public Side _Side;
    public enum Type { Parent, Child }
    public Type _Type;

    private Player player;
    public Player Player { get { return player == null ? player = GetComponentInParent<Player>() : player; } }

    private ChildZipper childZipper;
    public ChildZipper ChildZipper { get { return childZipper == null ? childZipper = GetComponentInParent<ChildZipper>() : childZipper; } }

    private Rigidbody rb;
    public Rigidbody Rigidbody { get { return rb == null ? rb = GetComponent<Rigidbody>() : rb; } }

    private Collider coll;
    public Collider Collider { get { return coll == null ? coll = GetComponent<Collider>() : coll; } }

    private GapMovement gapMovement;
    public GapMovement GapMovement => gapMovement == null ? gapMovement = FindObjectOfType<GapMovement>() : gapMovement;

    public bool CantMove
    {
        get
        {
            if (_Type == Type.Parent)
                return Player.IsDead;
            else if (_Type == Type.Child)
                return ChildZipper.IsDetached;
            else
                return false;
        }
    }

    private void OnEnable()
    {
        Rigidbody.isKinematic = true;
        //Collider.enabled = false;

        if (_Type == Type.Parent)
            Player.OnKill += Activate;
        else if (_Type == Type.Child)
            ChildZipper.OnActivateInnerZipperPair += Activate;
    }

    private void OnDisable()
    {
        if (_Type == Type.Parent && Player)
            Player.OnKill -= Activate;
        else if (_Type == Type.Child && ChildZipper)
            ChildZipper.OnActivateInnerZipperPair -= Activate;
    }

    private void Activate()
    {
        transform.parent = null;
        Rigidbody.isKinematic = false;
        //Collider.enabled = true;
        ApplyRandomForce();

        Destroy(gameObject, 20f);
        //Destroy(gameObject);
    }

    private void ApplyRandomForce()
    {
        if (_Type == Type.Parent)
            Rigidbody.AddForce(GenerateRandomForce() * Player.DetachmentForce, ForceMode.Impulse);
        else if (_Type == Type.Child)
            Rigidbody.AddForce(GenerateRandomForce() * ChildZipper.DetachmentForce, ForceMode.Impulse);
    }

    private Vector3 GenerateRandomForce() => new Vector3(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(1f, 2f), UnityEngine.Random.Range(-1f, 1f));

    private void OnCollisionEnter(Collision collision)
    {

        if (_Type == Type.Parent && Player.CurrentRow == 0 && collision.gameObject.TryGetComponent(out ObstacleBase obstacleForParent))
        {
            obstacleForParent.Execute();
            Player.KillTrigger();
        }

        if (_Type == Type.Child && !ChildZipper.IsDetached && collision.gameObject.TryGetComponent(out ObstacleBase obstacleForChild))
        {
            obstacleForChild.Execute();
            ChildZipper.Player.DetachZipperTrigger(ChildZipper.Row);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_Type == Type.Parent && other.TryGetComponent(out CollectableBase collectableForParent))
        {
            collectableForParent.Collect();
            Player.PickUpTrigger(collectableForParent.CollectableEffect);
        }

        if (_Type == Type.Child && !ChildZipper.IsDetached && other.TryGetComponent(out CollectableBase collectableForChild))
        {
            collectableForChild.Collect();
            ChildZipper.PickUpTrigger(collectableForChild.CollectableEffect);
        }
    }
}
