using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Kitchen : MonoBehaviour, IPurchasable
{
    [SerializeField] private int cost;

    public bool available;
    [SerializeField] private Rigidbody doorRb;
    [SerializeField] private List<Transform> longWalls = new List<Transform>();

    public int GetCost()
    {
        return cost;
    }

    public void GetPurchased()
    {
        doorRb.isKinematic = false;

        transform.DOMoveY(0, 1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            available = true;
            doorRb.isKinematic = false;
        });
        foreach (var longWall in longWalls)
        {
            longWall.DOMoveY(-4, 1f).SetEase(Ease.OutBack).OnComplete(()=> Destroy(longWall.gameObject));
        }
    }
}