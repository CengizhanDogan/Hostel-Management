using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coaches : Singleton<Coaches>, IPurchasable
{
    public bool available;
    private Vector3 scale;
    private PurchaseBehaviour purchaseBehaviour;
    private void Awake()
    {
        scale = transform.localScale;
        transform.localScale = Vector3.zero;
    }
    private void Start()
    {
        if (available)
        {
            purchaseBehaviour.Loaded();
            GetPurchased();
        }
    }
    public int GetCost(PurchaseBehaviour pb)
    {
        purchaseBehaviour = pb;
        return 500;
    }

    public void GetPurchased()
    {
        available = true;
        transform.DOScale(scale, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
        });
    }

    public bool IsPurchased()
    {
        return available;
    }

    public void SetBool(bool set)
    {
        available = set;
    }
}
