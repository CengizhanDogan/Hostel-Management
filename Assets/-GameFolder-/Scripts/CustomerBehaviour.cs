using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CustomerBehaviour : MonoBehaviour, IInteractable
{
    private StateMachine stateMachine;
    public float patiance;

    public Collider getColl;
    private bool interacted;

    private void Awake()
    {
        var navMeshAgent = GetComponent<NavMeshAgent>();

        stateMachine = new StateMachine();

        var receptionBehavior = new ReceptionBehaviour(navMeshAgent, transform, this);
        var wait = new Wait(this);
        var customerFolow = new CustomerFollow(navMeshAgent);

        At(receptionBehavior, wait, ReachedReception());
        At(wait, customerFolow, Getted());

        stateMachine.SetState(receptionBehavior);

        void At (IState to, IState from, Func<bool> predicate) => stateMachine.AddTransition(to, from, predicate);

        Func<bool> ReachedReception() => () => receptionBehavior.end;
        Func<bool> Getted() => () => interacted;
    }

    private void Update() => stateMachine.Tick();

    public void Interact(Interactor collector)
    {
        interacted = true;
        getColl.enabled = false;
    }
}
