using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Room : MonoBehaviour, IPurchasable
{
    private CustomerBehaviour roomCustomer;

    public int roomValue;

    public Door door;

    public bool available;
    public void SetCustomer(CustomerBehaviour customer) { roomCustomer = customer; }
    public CustomerBehaviour GetCustomer() { return roomCustomer; }

    [SerializeField] private Rigidbody doorRb;
    [SerializeField] private Transform longWalls;

    [SerializeField] private Transform sitTransform;
    [SerializeField] private Transform sleepTransform;

    public int GetCost(out int cost)
    {
        return cost = roomValue;
    }

    public void GetPurchased()
    {
        doorRb.isKinematic = false;

        transform.DOMoveY(0, 1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            available = true;
            doorRb.isKinematic = false;
        });
        longWalls.transform.DOMoveY(-3, 1f).SetEase(Ease.OutBack);
    }
}
