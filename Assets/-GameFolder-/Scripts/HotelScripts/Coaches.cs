using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coaches : MonoBehaviour, IPurchasable
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
        return 250;
    }

    public void GetPurchased()
    {
        transform.DOScale(scale, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            available = true;
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
