using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Reception : Singleton<Reception>, IPurchasable
{
    [SerializeField] private Transform waitTransform;
    [SerializeField] private Transform lookTransform;

    public List<CustomerBehaviour> customers = new List<CustomerBehaviour>();

    public bool available;

    private Vector3 scale;
    private PurchaseBehaviour purchaseBehaviour;

    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        scale = transform.localScale;
        if (available)
        {
            purchaseBehaviour.Loaded();
            GetPurchased();
        }
        else
        {
            transform.localScale = Vector3.zero;
        }
    }
    public void AddCustomer(CustomerBehaviour customer)
    {
        customers.Add(customer);
        if (customers.Count >= 2)
        {
            audioSource.Play();
        }
    }
    public Vector3 WaitPos(int offsetValue)
    {
        return waitTransform.position + (Vector3.right * 2f * offsetValue);
    }
    public Vector3 LookPos(float yPos)
    {
        var pos = lookTransform.position; pos.y = yPos;
        return pos;
    }
    public void RemoveCustomer(CustomerBehaviour customer)
    {
        customers.Remove(customer);
        if (customers.Count < 2)
        {
            audioSource.Stop();
        }
    }

    public int GetCost(PurchaseBehaviour pb)
    {
        purchaseBehaviour = pb;
        return 30;
    }

    public void GetPurchased()
    {
        available = true;
        transform.DOScale(scale, 0.5f).SetEase(Ease.OutBounce);
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
