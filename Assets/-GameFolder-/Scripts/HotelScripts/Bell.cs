using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bell : Singleton<Bell>
{
    private AudioSource bellRing;
    private SkinnedMeshRenderer rend;

    [SerializeField] private Transform exclamationBubble;

    private Vector3 bubbleScale;
    Vector3 scale;

    private void Start()
    {
        scale = transform.localScale;
        bellRing = GetComponent<AudioSource>();
        rend = GetComponent<SkinnedMeshRenderer>();
        bubbleScale = exclamationBubble.localScale;
        exclamationBubble.localScale = Vector3.zero;
    }
    public void RingBell()
    {
        transform.DOComplete(this);
        bellRing.Play();
        float blendValue = 0f;
        transform.localScale = scale;
        exclamationBubble.DOScale(bubbleScale, 0.5f).SetEase(Ease.OutBounce);

        transform.DOScale(scale + Vector3.one * 1.025f, 0.5f).SetEase(Ease.OutBounce);

        DOTween.To(() => blendValue, x => blendValue = x, 100, 0.25f)
            .OnUpdate(() => rend.SetBlendShapeWeight(0, blendValue))
            .OnComplete(() => DOTween.To(() => blendValue, x => blendValue = x, 0, 0.25f))
            .OnUpdate(() => rend.SetBlendShapeWeight(0, blendValue));
    }

    public void DestroyBubble()
    {
        DOTween.Kill(exclamationBubble);
        exclamationBubble.DOScale(Vector3.zero, 0.25f).SetEase(Ease.Linear);
    }
}
