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
    [SerializeField] private Rigidbody doorRb;
    [SerializeField] private List<Transform> longWalls = new List<Transform>();

    public Transform sitTransform;
    public Transform sleepTransform;

    public void SetCustomer(CustomerBehaviour customer)
    {
        roomCustomer = customer;
        door.coll.enabled = !customer;
    }
    public CustomerBehaviour GetCustomer() { return roomCustomer; }

    public int GetCost()
    {
        return roomValue;
    }

    public void GetPurchased()
    {
        doorRb.isKinematic = false;

        transform.DOMoveY(0, 1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            available = true;
            door.coll.enabled = true;
            doorRb.isKinematic = false;
        });
        foreach (var longWall in longWalls)
        {
            longWall.DOMoveY(-4, 1f).SetEase(Ease.OutBack);
        }
    }
}
