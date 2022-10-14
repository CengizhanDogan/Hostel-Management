using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Kitchen : Singleton<Kitchen>, IPurchasable
{
    [SerializeField] private int cost;

    public bool available;
    [SerializeField] private Rigidbody doorRb;
    [SerializeField] private List<Transform> longWalls = new List<Transform>();

    public Transform trashCan;
    public List<Fridge> fridges = new List<Fridge>();

    private PurchaseBehaviour purchaseBehaviour;

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
        return cost;
    }

    public void GetPurchased()
    {
        doorRb.isKinematic = false;
        available = true;

        transform.DOMoveY(0, 1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            doorRb.isKinematic = false;
        });
        foreach (var longWall in longWalls)
        {
            longWall.DOMoveY(-4, 1f).SetEase(Ease.OutBack).OnComplete(() => Destroy(longWall.gameObject));
        }
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
