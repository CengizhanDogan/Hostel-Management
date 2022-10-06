using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cloud : MonoBehaviour
{
    private Vector3 busySignScale;
    private Vector3 particleScale;
    [SerializeField] private Transform particle;
    void Start()
    {
        busySignScale = transform.localScale;
        transform.localScale = Vector3.zero;
        particleScale = particle.localScale;
        particle.localScale = Vector3.zero;
    }

    public void SetCloud(bool set)
    {
        Vector3 signScale;
        Vector3 pScale;

        if (set) { signScale = busySignScale; pScale = particleScale; }
        else { signScale = Vector3.zero; pScale = Vector3.zero; }

        transform.DOScale(signScale, 0.5f).SetEase(Ease.OutBounce);
        particle.DOScale(pScale, 0.5f).SetEase(Ease.OutBounce);
    }
}
