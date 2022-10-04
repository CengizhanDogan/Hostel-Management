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
        if(!reception.customers.Contains(customerBehaviour)) reception.AddCustomer(customerBehaviour);
        customerBehaviour.customerAnimation.SetWalk(true);
        Debug.Log(reception.customers.IndexOf(customerBehaviour));
        navMeshAgent.SetDestination(reception.WaitPos(customerBehaviour));
        customerBehaviour.getColl.enabled = false;
    }

    public void OnExit()
    {
        customerBehaviour.customerAnimation.SetWalk(false);
        transform.DOLookAt(reception.LookPos(transform.position.y), 0.5f);
        customerBehaviour.getColl.enabled = true;
    }

    public void Tick() 
    {
        if (!navMeshAgent.hasPath)
        {
            end = true;
        }
    }
}
