using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Room : MonoBehaviour, IPurchasable, ITimer
{
    private CustomerBehaviour roomCustomer;

    public int roomValue;

    public Door door;

    [SerializeField] private Timer timer;
    public bool available;
    [SerializeField] private Rigidbody doorRb;
    [SerializeField] private List<Transform> longWalls = new List<Transform>();

    public Transform sitTransform;
    public Transform sleepTransform;

    public Cloud cloud;
    [SerializeField] private Order order;

    private void Start()
    {
        order.room = this;
    }
    public void SetCustomer(CustomerBehaviour customer)
    {
        roomCustomer = customer;
        door.coll.enabled = !customer;
        if (customer) timer.StartTimer();
        else timer.StopTimer();
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
            longWall.DOMoveY(-4, 1f).SetEase(Ease.OutBack).OnComplete(() => Destroy(longWall.gameObject));
        }
    }

    public float Time()
    {
        var time = 0f;
        if (roomCustomer) time = roomCustomer.roomTime;
        return time;
    }

    public Color TargetColor()
    {
        return Color.green;
    }

    public void SetOrder(bool set)
    {
        order.SetBubbles(set);
        
    }
}
