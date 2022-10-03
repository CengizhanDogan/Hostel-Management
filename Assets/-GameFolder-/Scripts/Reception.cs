using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reception : Singleton<Reception>
{
    [SerializeField] private Transform waitTransform;
    [SerializeField] private Transform lookTransform;

    private List<CustomerBehaviour> customers = new List<CustomerBehaviour>();

    public void AddCustomer(CustomerBehaviour customer)
    {
        customers.Add(customer);
    }
    public Vector3 WaitPos()
    {
        return waitTransform.position + (Vector3.right * -1.5f * customers.Count);
    }
    public Vector3 LookPos(float yPos)
    {
        var pos = lookTransform.position; pos.y = yPos;
        return pos;
    }
    public void RemoveCustomer(CustomerBehaviour customer)
    {
        customers.Remove(customer);
    }
}
