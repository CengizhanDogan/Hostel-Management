using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomBehaviour : MonoBehaviour, IState
{
    private StateMachine stateMachine;

    private CustomerBehaviour customerBehaviour;
    private Room room;
    private float roomTime;
    private NavMeshAgent navMeshAgent;

    private bool firstMovement;
    private bool tick;

    public void SetRoom(Room room) { this.room = room; }
    public void SetRoomTime(float time) { roomTime = time; }
    public void SetCustomerBehaviour(CustomerBehaviour customerBehaviour) { this.customerBehaviour = customerBehaviour; }

    public void OnEnter()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        firstMovement = true;
        customerBehaviour.customerAnimation.SetWalk(true);
        navMeshAgent.SetDestination(room.transform.position);
    }

    public void OnExit() { }

    public void Tick()
    {
        if (firstMovement && !navMeshAgent.hasPath)
        {
            firstMovement = false;
            tick = true;
            SetStateMachine();
        }
        if (tick)
        {
            stateMachine.Tick();
        }

        
    }

    private void SetStateMachine()
    {
        customerBehaviour.customerAnimation.SetWalk(false);

        stateMachine = new StateMachine();

        var wonderRoom = new WonderRoom(navMeshAgent, room, customerBehaviour);

        stateMachine.SetState(wonderRoom);

        //stateMachine.AddAnyTransition(wonderRoom, Wonder());
        //void At(IState to, IState from, Func<bool> predicate) => stateMachine.AddTransition(to, from, predicate);

        //Func<bool> Sleep() => () => !navMeshAgent.hasPath && UnityEngine.Random.value < 0.7f;
    }
}

public class WonderRoom : IState
{
    private NavMeshAgent navMeshAgent;
    private CustomerBehaviour customerBehaviour;
    private Room room;

    private float wonderValue;

    public WonderRoom(NavMeshAgent navMeshAgent, Room room, CustomerBehaviour customerBehaviour)
    {
        this.navMeshAgent = navMeshAgent;
        this.room = room;
        this.customerBehaviour = customerBehaviour;
    }
    public void OnEnter()
    {
        Wonder();
    }

    public void OnExit()
    {
        customerBehaviour.customerAnimation.SetWalk(false);
    }

    public void Tick()
    {
        if (!navMeshAgent.hasPath)
        {
            customerBehaviour.customerAnimation.SetWalk(false);
            wonderValue = UnityEngine.Random.value;
            if (wonderValue > 0.995f) Wonder();
        }
    }

    private void Wonder()
    {
        customerBehaviour.customerAnimation.SetWalk(true);
        var randomPos = room.transform.position;
        randomPos.x += UnityEngine.Random.Range(-2f, 2f);
        randomPos.z += UnityEngine.Random.Range(-2f, 2f);
        navMeshAgent.SetDestination(randomPos);
    }
}
