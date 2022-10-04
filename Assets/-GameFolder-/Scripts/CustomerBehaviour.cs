using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CustomerBehaviour : MonoBehaviour, IInteractable
{
    public CustomerAnimation customerAnimation;
    private StateMachine stateMachine;
    private RoomBehaviour roomBehaviour;
    private Wait wait;
    public float patiance;
    [SerializeField] private float roomTime;

    public Collider getColl;
    private bool interacted;
    private bool hasRoom;

    private void Awake()
    {
        customerAnimation = GetComponent<CustomerAnimation>();
        var navMeshAgent = GetComponent<NavMeshAgent>();

        roomBehaviour = gameObject.AddComponent<RoomBehaviour>();
        roomBehaviour.SetRoomTime(roomTime);
        roomBehaviour.SetCustomerBehaviour(this);

        stateMachine = new StateMachine();

        var receptionBehavior = new ReceptionBehaviour(navMeshAgent, this);
        wait = new Wait(this , receptionBehavior);
        var customerFolow = new CustomerFollow(navMeshAgent, this);

        At(receptionBehavior, wait, ReachedReception());
        At(wait, receptionBehavior, Reorder());
        At(wait, customerFolow, Getted());
        At(customerFolow, roomBehaviour, HasRoom());

        stateMachine.SetState(receptionBehavior);

        void At(IState to, IState from, Func<bool> predicate) => stateMachine.AddTransition(to, from, predicate);

        Func<bool> ReachedReception() => () => receptionBehavior.end;
        Func<bool> Reorder() => () => !receptionBehavior.end;
        Func<bool> Getted() => () => interacted;
        Func<bool> HasRoom() => () => hasRoom;
    }

    private void Update() => stateMachine.Tick();

    public void Interact(ManagerBehaviour manager)
    {
        interacted = true;
        getColl.enabled = false;
        manager.SetCustomer(this);
        Reception.Instance.RemoveCustomer(this);
        foreach (var customer in Reception.Instance.customers)
        {
            customer.SetReorder();
        }
    }

    public void SetToRoom(Room room)
    {
        roomBehaviour.SetRoom(room);
        hasRoom = true;
    }

    public void SetReorder()
    {
        wait.reorder = true;
    }
}
