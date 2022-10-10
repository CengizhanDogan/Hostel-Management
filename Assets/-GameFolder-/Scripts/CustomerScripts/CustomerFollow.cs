using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerFollow : IState
{
    private NavMeshAgent navMeshAgent;
    public CustomerGetter manager;
    private CustomerBehaviour customerBehaviour;
    private Timer timer;

    public CustomerFollow(NavMeshAgent navMeshAgent, CustomerBehaviour customerBehaviour, Timer timer)
    {
        this.navMeshAgent = navMeshAgent;
        this.customerBehaviour = customerBehaviour;
        this.timer = timer;
    }
    public void OnEnter() { timer.StopTimer(); }

    public void OnExit() { navMeshAgent.isStopped = false; }

    public void Tick()
    {
        var distance = Vector3.Distance(navMeshAgent.transform.position, manager.transform.position);

        if (distance > 2.5f)
        {
            navMeshAgent.isStopped = false;
            customerBehaviour.customerAnimation.SetWalk(true);
            navMeshAgent.SetDestination(GetManagerPos());
        }
        else
        {
            customerBehaviour.customerAnimation.SetWalk(false);
            navMeshAgent.isStopped = true;
        }
    }

    private Vector3 GetManagerPos()
    {
        return manager.transform.position;
    }
}
