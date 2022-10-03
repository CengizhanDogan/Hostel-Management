using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerFollow : IState
{
    private NavMeshAgent navMeshAgent;
    private ManagerBehaviour manager;
    private CustomerBehaviour customerBehaviour;

    public CustomerFollow(NavMeshAgent navMeshAgent, CustomerBehaviour customerBehaviour)
    {
        this.navMeshAgent = navMeshAgent;
        this.customerBehaviour = customerBehaviour;
        manager = ManagerBehaviour.Instance;
    }
    public void OnEnter() { }

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
