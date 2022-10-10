using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coaches : MonoBehaviour, IPurchasable
{
    public bool available;
    private Vector3 scale;

    private void Awake()
    {
        scale = transform.localScale;
        transform.localScale = Vector3.zero;
    }
    public int GetCost()
    {
        return 50;
    }

    public void GetPurchased()
    {
        transform.DOScale(scale, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            available = true;
        });
    }
}
