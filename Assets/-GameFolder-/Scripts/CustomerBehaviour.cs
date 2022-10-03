using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CustomerBehaviour : MonoBehaviour, IInteractable
{
    private StateMachine stateMachine;
    private RoomBehaviour roomBehaviour;
    public float patiance;
    [SerializeField] private float roomTime;

    public Collider getColl;
    private bool interacted;
    private bool hasRoom;

    private void Awake()
    {
        var navMeshAgent = GetComponent<NavMeshAgent>();

        roomBehaviour = gameObject.AddComponent<RoomBehaviour>();
        roomBehaviour.SetRoomTime(roomTime);

        stateMachine = new StateMachine();

        var receptionBehavior = new ReceptionBehaviour(navMeshAgent, this);
        var wait = new Wait(this);
        var customerFolow = new CustomerFollow(navMeshAgent);

        At(receptionBehavior, wait, ReachedReception());
        At(wait, customerFolow, Getted());
        At(customerFolow, roomBehaviour, HasRoom());

        stateMachine.SetState(receptionBehavior);

        void At (IState to, IState from, Func<bool> predicate) => stateMachine.AddTransition(to, from, predicate);

        Func<bool> ReachedReception() => () => receptionBehavior.end;
        Func<bool> Getted() => () => interacted;
        Func<bool> HasRoom() => () => hasRoom;
    }

    private void Update() => stateMachine.Tick();

    public void Interact(ManagerBehaviour manager)
    {
        interacted = true;
        getColl.enabled = false;
        manager.SetCustomer(this);
    }

    public void SetToRoom(Room room) 
    {
        roomBehaviour.SetRoom(room);
        hasRoom = true; 
    }
}
