using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class ReceptionBehaviour : IState
{
    private Reception reception;
    private NavMeshAgent navMeshAgent;
    private Transform transform;
    private CustomerBehaviour customerBehaviour;
    public bool end;

    public ReceptionBehaviour(NavMeshAgent navMeshAgent, CustomerBehaviour customerBehaviour)
    {
        this.navMeshAgent = navMeshAgent;
        this.transform = customerBehaviour.transform;
        this.customerBehaviour = customerBehaviour;

        reception = Reception.Instance;
    }
    public void OnEnter()
    {
        customerBehaviour.customerAnimation.SetWalk(true);
        navMeshAgent.SetDestination(reception.WaitPos());
    }

    public void OnExit()
    {
        customerBehaviour.customerAnimation.SetWalk(false);
        transform.DOLookAt(reception.LookPos(transform.position.y), 0.5f);
        reception.AddCustomer(customerBehaviour);
    }

    public void Tick() 
    {
        if (!navMeshAgent.hasPath)
        {
            end = true;
        }
    }
}
