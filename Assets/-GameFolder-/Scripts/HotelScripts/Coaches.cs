using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Coaches : Singleton<Coaches>, IPurchasable
{
    public bool available;
    private Vector3 scale;
    private PurchaseBehaviour purchaseBehaviour;
    public List<CoachSeat> seats = new List<CoachSeat>();
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

    public CoachSeat EmptySeat()
    {
        foreach (CoachSeat seat in seats)
        {
            if (seat.customerBehaviour == null)
            {
                return seat;
            }
        }

        return null;
    }
    public CoachSeat CustomerSeat()
    {
        foreach (CoachSeat seat in seats)
        {
            if (seat.customerBehaviour)
            {
                return seat;
            }
        }

        return null;
    }

    public bool HasCustomer(CustomerBehaviour customer)
    {
        foreach (CoachSeat seat in seats)
        {
            if (seat.customerBehaviour == customer)
            {
                return true;
            }
        }

        return false;
    }
}
[Serializable]
public class CoachSeat
{
    public Transform seatTransform;
    public CustomerBehaviour customerBehaviour;
}
