using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reception : Singleton<Reception>
{
    [SerializeField] private Transform waitTransform;
    [SerializeField] private Transform lookTransform;

    public List<CustomerBehaviour> customers = new List<CustomerBehaviour>();

    public void AddCustomer(CustomerBehaviour customer)
    {
        customers.Add(customer);
    }
    public Vector3 WaitPos(CustomerBehaviour customer)
    {
        var offsetValue = customers.IndexOf(customer);
        //Debug.Log(offsetValue);
        return waitTransform.position + (Vector3.forward * -1f * offsetValue) + (Vector3.right * 1.8f * offsetValue);
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
