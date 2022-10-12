using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : IState
{
    private CustomerBehaviour customerBehaviour;
    private ReceptionBehaviour reception;
    private Timer timer;

    public bool reorder;
    private bool timerStarted; 
    public Wait(CustomerBehaviour customerBehaviour, ReceptionBehaviour reception, Timer timer)
    {
        this.customerBehaviour = customerBehaviour;
        this.reception = reception;
        this.timer = timer;
    }

    public void OnEnter() 
    {
        if (!timerStarted)
        {
            timer.StartTimer();
            timerStarted = true;
        }
        customerBehaviour.getColl.enabled = true;
    }

    public void OnExit() 
    {
        Bell.Instance.DestroyBubble();
    }

    public void Tick()
    {
        customerBehaviour.patiance -= Time.deltaTime;
        if (reorder)
        {
            reorder = false;
            reception.end = false;
        }
        if (customerBehaviour.patiance <= 0)
        {
            timer.StopTimer();
            customerBehaviour.exit = true;
        }
    }
}
