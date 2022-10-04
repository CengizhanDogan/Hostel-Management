using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : IState
{
    private CustomerBehaviour customerBehaviour;
    private ReceptionBehaviour reception;

    public bool reorder;
    public Wait(CustomerBehaviour customerBehaviour, ReceptionBehaviour reception)
    {
        this.customerBehaviour = customerBehaviour;
        this.reception = reception;
    }

    public void OnEnter() 
    {
        customerBehaviour.getColl.enabled = true;
    }

    public void OnExit() 
    {
        
    }

    public void Tick()
    {
        customerBehaviour.patiance -= 0.01f;
        if (reorder)
        {
            reorder = false;
            reception.end = false;
        }
    }
}
