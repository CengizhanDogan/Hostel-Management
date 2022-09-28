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

    public ReceptionBehaviour(NavMeshAgent navMeshAgent, Transform transform, CustomerBehaviour customerBehaviour)
    {
        this.navMeshAgent = navMeshAgent;
        this.transform = transform;
        this.customerBehaviour = customerBehaviour;

        reception = Reception.Instance;
    }
    public void OnEnter()
    {
        navMeshAgent.SetDestination(reception.WaitPos());
    }

    public void OnExit()
    {
        transform.DORotate(Vector3.up * -270, 0.5f);
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
