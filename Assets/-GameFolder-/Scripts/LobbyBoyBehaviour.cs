using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class LobbyBoyBehaviour : MonoBehaviour, IPurchasable
{
    private StateMachine stateMachine;

    [HideInInspector] public bool get;
    [HideInInspector] public bool wait;
    [HideInInspector] public bool go;
    private bool purchased;

    public Animator anim;

    void Awake()
    {
        var startPos = transform.position;
        var navMeshAgent = GetComponent<NavMeshAgent>();

        stateMachine = new StateMachine();

        var waitBehaviour = new LobbyBoyWait(this, navMeshAgent, startPos);
        var getCustomer = new GetCustomer(this, navMeshAgent);
        var goToRoom = new GoToRoom(this, navMeshAgent);

        At(waitBehaviour, getCustomer, DoGet());
        At(getCustomer, goToRoom, DoGo());
        At(goToRoom, waitBehaviour, DoWait());

        stateMachine.SetState(waitBehaviour);

        void At(IState to, IState from, Func<bool> predicate) => stateMachine.AddTransition(to, from, predicate);

        Func<bool> DoGet() => () => get && purchased;
        Func<bool> DoGo() => () => go;
        Func<bool> DoWait() => () => !go || !get;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CustomerBehaviour cb))
        {
            go = true;
        }
    }
    void Update() { stateMachine.Tick(); }

    public int GetCost()
    {
        return 100;
    }

    public void GetPurchased()
    {
        transform.DOScale(1, 0.5f).SetEase(Ease.OutBounce)
            .OnComplete(() => 
            { 
                GetComponent<NavMeshAgent>().enabled = true; 
                purchased = true; 
            });
    }
}

public class GetCustomer : IState
{
    private LobbyBoyBehaviour lobbyBoy;
    private NavMeshAgent navMeshAgent;
    public GetCustomer(LobbyBoyBehaviour lobbyBoy, NavMeshAgent navMeshAgent)
    {
        this.lobbyBoy = lobbyBoy;
        this.navMeshAgent = navMeshAgent;
    }

    public void OnEnter()
    {
        lobbyBoy.anim.SetBool("Walk", true);
        navMeshAgent.SetDestination(Reception.Instance.customers[0].transform.position);
    }

    public void OnExit()
    {

    }

    public void Tick()
    {

    }
}
public class LobbyBoyWait : IState
{
    private LobbyBoyBehaviour lobbyBoy;
    private NavMeshAgent navMeshAgent;

    private bool hasRoom;
    private bool turn;
    private bool check;
    private bool doOnce;


    private Vector3 startPos;

    public LobbyBoyWait(LobbyBoyBehaviour lobbyBoy, NavMeshAgent navMeshAgent, Vector3 startPos)
    {
        this.lobbyBoy = lobbyBoy;
        this.navMeshAgent = navMeshAgent;
        this.startPos = startPos;
    }

    public void OnEnter()
    {
        lobbyBoy.anim.SetBool("Walk", true);
        if(navMeshAgent.enabled)navMeshAgent.SetDestination(startPos);
        if (!doOnce)
        {
            doOnce = true;
            turn = true;
        }
    }
    public void OnExit()
    {
        turn = false;
        check = false;
        hasRoom = false;
    }

    public void Tick()
    {
        if (navMeshAgent.hasPath && !check)
        {
            check = true;
            turn = true;
        }
        if (!navMeshAgent.hasPath && turn)
        {
            turn = false;
            lobbyBoy.anim.SetBool("Walk", false);
            lobbyBoy.transform.DORotate(Vector3.up * 180, 0.5f);
        }
        foreach (var room in RoomLister.Instance.rooms)
        {
            if (room.available && !room.GetCustomer())
            {
                hasRoom = true;
            }
        }
        if (hasRoom && Reception.Instance.customers.Count > 0)
        {
            if (Reception.Instance.customers[0].getColl.enabled)
                lobbyBoy.get = true;
        }
    }
}

public class GoToRoom : IState
{
    private LobbyBoyBehaviour lobbyBoy;
    private NavMeshAgent navMeshAgent;

    private bool check;
    public GoToRoom(LobbyBoyBehaviour lobbyBoy, NavMeshAgent navMeshAgent)
    {
        this.lobbyBoy = lobbyBoy;
        this.navMeshAgent = navMeshAgent;
    }
    public void OnEnter()
    {
        foreach (var room in RoomLister.Instance.rooms)
        {
            if (room.available && !room.GetCustomer())
            {
                navMeshAgent.SetDestination(room.door.transform.position);
                check = true;
                lobbyBoy.anim.SetBool("Walk", true);
                return;
            }
        }
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        if (check)
        {
            if (!navMeshAgent.hasPath)
            {
                lobbyBoy.get = false;
                lobbyBoy.go = false;
            }
        }
    }
}
