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
        if (!reception.customers.Contains(customerBehaviour)) reception.AddCustomer(customerBehaviour);
        customerBehaviour.customerAnimation.SetWalk(true);
        navMeshAgent.SetDestination(reception.WaitPos(reception.customers.IndexOf(customerBehaviour)));
        customerBehaviour.getColl.enabled = false;
    }

    public void OnExit()
    {
        customerBehaviour.areaTransform.DOScale(0.5f, 0.6f).SetEase(Ease.OutBack);
        customerBehaviour.customerAnimation.SetWalk(false);
        transform.DOLookAt(reception.LookPos(transform.position.y), 0.25f);
        customerBehaviour.getColl.enabled = true;
        if (reception.customers.IndexOf(customerBehaviour) == 0) Bell.Instance.RingBell();
    }

    public void Tick()
    {
        if (!navMeshAgent.hasPath)
        {
            end = true;
        }
    }
}
