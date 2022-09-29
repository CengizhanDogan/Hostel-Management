using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : IState
{
    private CustomerBehaviour customerBehaviour;
    public Wait(CustomerBehaviour customerBehaviour)
    {
        this.customerBehaviour = customerBehaviour;
    }

    public void OnEnter() 
    {
        customerBehaviour.getColl.enabled = true;
    }

    public void OnExit() 
    {
        Reception.Instance.RemoveCustomer(customerBehaviour);
    }

    public void Tick()
    {
        customerBehaviour.patiance -= 0.01f;
    }
}
