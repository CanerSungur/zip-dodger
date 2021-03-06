using UnityEngine;
using DG.Tweening;

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
            Player.OnKill += Detach;
        else if (_Type == Type.Child)
            ChildZipper.OnActivateInnerZipperPair += Detach;
    }

    private void OnDisable()
    {
        if (_Type == Type.Parent && Player)
            Player.OnKill -= Detach;
        else if (_Type == Type.Child && ChildZipper)
            ChildZipper.OnActivateInnerZipperPair -= Detach;

        transform.DOKill();
    }

    private void Detach()
    {
        transform.parent = null;
        Rigidbody.isKinematic = false;
        //Collider.enabled = true;
        
        if (Player.FinishedPlatform)
        {
            //jump
            if (_Side == Side.Left)
                transform.DOJump(new Vector3(transform.position.x - 3f, transform.position.y, transform.position.z), 3f, 1, 2f);
            else if (_Side == Side.Right)
                transform.DOJump(new Vector3(transform.position.x + 3f, transform.position.y, transform.position.z), 3f, 1, 2f);
        }
        else
        {
            ApplyRandomForce();

            Destroy(gameObject, 20f);
            //Destroy(gameObject);
        }
    }

    private void ApplyRandomForce()
    {
        if (_Type == Type.Parent)
            Rigidbody.AddForce(GenerateRandomForce() * Player.DetachmentForce, ForceMode.Impulse);
        else if (_Type == Type.Child)
            Rigidbody.AddForce(GenerateRandomForce() * ChildZipper.DetachmentForce, ForceMode.Impulse);
    }

    public void DetachForPlatformEnd()
    {
        if (_Type == Type.Child)
        {
            transform.parent = null;
            ChildZipper.IsDetached = ChildZipper.Rigidbody.isKinematic = true;
            ChildZipper.childZipperMovement.enabled = false;
            
            // TODO: Start animation of zipppers.
            if (_Side == Side.Left)
                transform.DOJump(new Vector3(transform.position.x - 3f, transform.position.y, transform.position.z + 2f), 3f, 1, 2f);
            else if (_Side == Side.Right)
                transform.DOJump(new Vector3(transform.position.x + 3f, transform.position.y, transform.position.z + 2f), 3f, 1, 2f);

            CameraManager.UpdatePositionTrigger();
        }
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
        if (other.CompareTag("Finish") && !Player.FinishedPlatform)
        {
            //Player.FinishedPlatform = true;
            GameManager.PlatformEndTrigger();
        }

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
