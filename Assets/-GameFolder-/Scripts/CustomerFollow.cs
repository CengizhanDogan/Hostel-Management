using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerFollow : IState
{
    private NavMeshAgent navMeshAgent;
    private ManagerBehaviour manager;

    public CustomerFollow(NavMeshAgent navMeshAgent)
    {
        this.navMeshAgent = navMeshAgent;

        manager = ManagerBehaviour.Instance;
    }
    public void OnEnter() { }

    public void OnExit() { }

    public void Tick()
    {
        var distance = Vector3.Distance(navMeshAgent.transform.position, manager.transform.position);

        if (distance > 2.5f)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(GetManagerPos());
        }
        else
        {
            navMeshAgent.isStopped = true;
        }
    }

    private Vector3 GetManagerPos()
    {
        return manager.transform.position;
    }
}
