using UnityEngine;

public abstract class ObstacleBase : MonoBehaviour, IObstacle
{
    [SerializeField, Tooltip("Toggle this if this obstacle gets destroyed when hit.")] private bool disposable = false;
    [SerializeField, Tooltip("Particle effect of hitting this object.")] protected GameObject obstacleEffect;

    private Vector3 hitPoint = Vector3.zero;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zipper"))
        {
            hitPoint = collision.GetContact(0).point;
        }
    }

    public virtual void Execute()
    {
        CameraManager.ShakeCamTrigger();

        if (obstacleEffect)
        {
            ParticleSystem ps = Instantiate(obstacleEffect, hitPoint, Quaternion.Euler(0f, 180f, 0f)).GetComponent<ParticleSystem>();
            ps.Play();
            //Destroy(ps.gameObject, ps.main.duration);
        }

        Dispose();
    }

    public void Dispose()
    {
        if (disposable)
            Destroy(gameObject);
    }
}
