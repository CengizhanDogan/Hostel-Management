using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reception : Singleton<Reception>
{
    [SerializeField] private Transform waitTransform;

    private List<CustomerBehaviour> customers = new List<CustomerBehaviour>();

    public void AddCustomer(CustomerBehaviour customer)
    {
        customers.Add(customer);
    }
    public Vector3 WaitPos()
    {
        return waitTransform.position + (Vector3.right * -1.5f * customers.Count);
    }
}
