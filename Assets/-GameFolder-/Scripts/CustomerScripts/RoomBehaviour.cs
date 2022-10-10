using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class RoomBehaviour : MonoBehaviour, IState
{
    private StateMachine stateMachine;

    private CustomerBehaviour customerBehaviour;
    private Room room;
    [HideInInspector] public float roomTime;
    private NavMeshAgent navMeshAgent;

    public Sleep sleep;

    private bool firstMovement;
    private bool tick;
    public bool slept;
    public bool doSleep;

    public void SetRoom(Room room) { this.room = room; }
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
            customerBehaviour.roomTime -= Time.deltaTime;
            stateMachine.Tick();
        }
        if (customerBehaviour.roomTime <= 0)
        {
            customerBehaviour.exit = true;
            customerBehaviour.giveMoney = true;
        }
    }

    private void SetStateMachine()
    {
        customerBehaviour.customerAnimation.SetWalk(false);

        stateMachine = new StateMachine();

        var wonderRoom = new WonderRoom(navMeshAgent, room, customerBehaviour, this);
        sleep = new Sleep(customerBehaviour, room, navMeshAgent, transform, this);

        At(wonderRoom, sleep, Sleep());
        At(sleep, wonderRoom, Slept());

        stateMachine.SetState(wonderRoom);

        //stateMachine.AddAnyTransition(wonderRoom, Wonder());
        void At(IState to, IState from, Func<bool> predicate) => stateMachine.AddTransition(to, from, predicate);

        Func<bool> Sleep() => () => doSleep;
        Func<bool> Slept() => () => slept;
    }
}

public class WonderRoom : IState
{
    private NavMeshAgent navMeshAgent;
    private CustomerBehaviour customerBehaviour;
    private Room room;
    private RoomBehaviour roomBehaviour;

    private float startTime;

    private float wonderValue;

    public WonderRoom(NavMeshAgent navMeshAgent, Room room, CustomerBehaviour customerBehaviour, RoomBehaviour roomBehaviour)
    {
        this.navMeshAgent = navMeshAgent;
        this.room = room;
        this.customerBehaviour = customerBehaviour;
        this.roomBehaviour = roomBehaviour;
    }
    public void OnEnter()
    {
        startTime = roomBehaviour.roomTime;
        roomBehaviour.slept = true;
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

            /* Wonder with probability
             * 
            if (wonderValue > 0.995f) Wonder();
            if (wonderValue < 0.0005f)
            {
                roomBehaviour.doSleep = true;
                roomBehaviour.slept = false;
            }

            */

            if (roomBehaviour.roomTime > startTime / 1.25f) Wonder();
            else
            {
                roomBehaviour.doSleep = true;
                roomBehaviour.slept = false;
            }
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

public class Sleep : IState
{
    private CustomerBehaviour customerBehaviour;
    private Room room;
    private NavMeshAgent navMeshAgent;
    private Transform transform;
    private RoomBehaviour roomBehaviour;
    private Vector3 startPos;

    private bool animating;
    private bool sleep;
    public bool sat;
    private float sleptValue;

    public Sleep(CustomerBehaviour customerBehaviour, Room room, NavMeshAgent navMeshAgent, Transform transform, RoomBehaviour roomBehaviour)
    {
        this.customerBehaviour = customerBehaviour;
        this.room = room;
        this.navMeshAgent = navMeshAgent;
        this.transform = transform;
        this.roomBehaviour = roomBehaviour;
    }
    public void OnEnter()
    {
        navMeshAgent.SetDestination(room.sitTransform.position);
        room.cloud.SetCloud(true);
    }

    public void OnExit()
    {
        transform.position = startPos;
        navMeshAgent.enabled = true;
        sleep = false;
        roomBehaviour.slept = true;
        roomBehaviour.doSleep = false;
        animating = false;
        customerBehaviour.customerAnimation.SetSleep(false);
    }

    public void Tick()
    {
        if (!navMeshAgent.hasPath && !animating)
        {
            startPos = transform.position;
            animating = true;
            navMeshAgent.enabled = false;
            transform.DORotate(room.sitTransform.eulerAngles, 0.5f);
            transform.DOMove(room.sitTransform.position, 0.5f).OnComplete(() =>
            {
                customerBehaviour.customerAnimation.SetSleep(true);
            });
        }
        if (sat)
        {
            sat = false;
            transform.DORotate(room.sleepTransform.eulerAngles, 0.5f);
            transform.DOMove(room.sleepTransform.position, 0.5f).OnComplete(() =>
            {
                sleep = true;
            });
        }
        sleptValue = UnityEngine.Random.value;

        // Don't sleep with random value

        //if (sleep && sleptValue < 0.0005f)
        //{
        //    sleep = false;
        //    customerBehaviour.customerAnimation.SetSleep(false);
        //    navMeshAgent.enabled = true;
        //    roomBehaviour.slept = true;
        //    roomBehaviour.doSleep = false;
        //}
    }
}
