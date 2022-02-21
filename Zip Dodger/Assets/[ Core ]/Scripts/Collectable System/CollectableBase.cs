using UnityEngine;
using DG.Tweening;
using System.Collections;

public abstract class CollectableBase : MonoBehaviour, ICollectable
{
    [Tooltip("Is this object collectable in scene or rewarded when something happens.")] public CollectableStyle CollectableStyle;
    [Tooltip("Select movement type of this object when it's collected.")] public CollectStyle CollectStyle;
    [Tooltip("The effect of this collectable on pick up.")] public CollectableEffect CollectableEffect;
    [SerializeField, Tooltip("Particle effect for collecting this object.")] protected GameObject collectEffect;

    private float bounceDuration = 0.3f;

    /// <summary>
    /// If you override this function, write everything you want before base.Collect();
    /// </summary>
    public virtual void Collect()
    {
        // You can add sound here.

        if (collectEffect)
        {
            ParticleSystem ps = Instantiate(collectEffect, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            ps.Play();
            //Destroy(ps.gameObject, ps.main.duration);
        }

        Apply();
        Dispose();
    }

    public abstract void Apply();

    public void Dispose()
    {
        if (CollectStyle == CollectStyle.OnSite)
        {
            Bounce();
            StartCoroutine(DestroyWithDelay(bounceDuration));
        }
    }

    private void Bounce()
    {
        //transform.DOShakePosition(bounceDuration * 0.5f, .5f);
        //transform.DOShakeRotation(bounceDuration * 0.5f, .5f);
        //transform.DOShakeScale(bounceDuration * 0.5f, .75f).OnComplete(() => {
        //    transform.DOScale(0, bounceDuration * 0.5f).SetEase(Ease.InBounce);
        //});
        transform.DOScale(0, bounceDuration).SetEase(Ease.InBounce);
    }

    private IEnumerator DestroyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        transform.DOKill();
        Destroy(gameObject);
    }
}
public enum CollectStyle { OnSite, MoveToUI }
public enum CollectableStyle { Collect, Reward }
public enum CollectableEffect { Positive, Negative }
