using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class LobbyBoyBehaviour : MonoBehaviour, IPurchasable
{
    private StateMachine stateMachine;

    public bool get;
    public bool wait;
    public bool go;
    private bool purchased;

    private PurchaseBehaviour purchaseBehaviour;
    public Animator anim;
    [HideInInspector] public CustomerGetter customerGetter;

    void Awake()
    {
        customerGetter = GetComponent<CustomerGetter>();
        var startPos = transform.position;
        var navMeshAgent = GetComponent<NavMeshAgent>();

        stateMachine = new StateMachine();

        var waitBehaviour = new LobbyBoyWait(this, navMeshAgent, startPos);
        var getCustomer = new GetCustomer(this, navMeshAgent);
        var goToRoom = new GoToRoom(this, navMeshAgent);

        At(waitBehaviour, getCustomer, DoGet());
        At(getCustomer, goToRoom, DoGo());
        At(waitBehaviour, goToRoom, DoGo());
        At(goToRoom, waitBehaviour, DoWait());
        At(getCustomer, waitBehaviour, DoWait());

        stateMachine.SetState(waitBehaviour);

        void At(IState to, IState from, Func<bool> predicate) => stateMachine.AddTransition(to, from, predicate);

        Func<bool> DoGet() => () => get && navMeshAgent.enabled;
        Func<bool> DoGo() => () => go && navMeshAgent.enabled;
        Func<bool> DoWait() => () => wait && navMeshAgent.enabled;
    }

    public bool HasRoom
    {
        get
        {
            foreach (var room in RoomLister.Instance.rooms)
            {
                if (room.available && !room.GetCustomer())
                {
                    return true;
                }
            }
            return false;
        }
    }

    #region
    private void Start()
    {
        if (purchased)
        {
            purchaseBehaviour.Loaded();
            GetPurchased();
        }
    }
    void Update() { stateMachine.Tick(); }

    public int GetCost(PurchaseBehaviour pb)
    {
        purchaseBehaviour = pb;
        return 400;
    }

    public void GetPurchased()
    {
        transform.DOScale(1, 0.5f).SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                GetComponent<NavMeshAgent>().enabled = true;
            });
        purchased = true;
    }

    public bool IsPurchased()
    {
        return purchased;
    }

    public void SetBool(bool set)
    {
        purchased = set;
    }
    #endregion
}

public class LobbyBoyWait : IState
{
    private LobbyBoyBehaviour lobbyBoy;
    private NavMeshAgent navMeshAgent;

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
        if (Vector3.Distance(lobbyBoy.transform.position, startPos) > 0.1f)
            lobbyBoy.anim.SetBool("Walk", true);
        if (navMeshAgent.enabled) navMeshAgent.SetDestination(startPos);
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
    }

    public void Tick()
    {
        if (lobbyBoy.HasRoom && lobbyBoy.customerGetter.GetCustomer())
        {
            lobbyBoy.wait = false;
            lobbyBoy.go = true;
            return;
        }
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
        if (lobbyBoy.HasRoom && Reception.Instance.customers.Count > 0)
        {
            if (Reception.Instance.customers[0].getColl.enabled)
            {
                lobbyBoy.wait = false;
                lobbyBoy.get = true;
                return;
            }
        }
        if (lobbyBoy.HasRoom && Coaches.Instance.CustomerSeat() != null)
        {
            if (Coaches.Instance.CustomerSeat().customerBehaviour.getColl.enabled)
            {
                lobbyBoy.wait = false;
                lobbyBoy.get = true;
                return;
            }
        }
    }
}

public class GetCustomer : IState
{
    private LobbyBoyBehaviour lobbyBoy;
    private NavMeshAgent navMeshAgent;
    private Coaches coaches;
    private Reception reception;

    private CustomerBehaviour customer;
    private bool inReception;
    private bool coach;
    public GetCustomer(LobbyBoyBehaviour lobbyBoy, NavMeshAgent navMeshAgent)
    {
        this.lobbyBoy = lobbyBoy;
        this.navMeshAgent = navMeshAgent;
        coaches = Coaches.Instance;
        reception = Reception.Instance;
    }

    public void OnEnter()
    {
        if (reception.customers.Count > 0 && reception.customers[0].getColl.enabled)
        {
            customer = reception.customers[0];
            navMeshAgent.SetDestination(customer.transform.position);
            lobbyBoy.anim.SetBool("Walk", true);
            inReception = true;
        }
        else if (coaches.CustomerSeat() != null)
        {
            var seat = coaches.CustomerSeat();
            customer = seat.customerBehaviour;
            navMeshAgent.SetDestination(seat.seatTransform.position);
            lobbyBoy.anim.SetBool("Walk", true);
            coach = true;
        }
        else
        {
            lobbyBoy.get = false;
        }
    }

    public void OnExit() { }

    public void Tick()
    {
        if (!customer || !lobbyBoy.HasRoom)
        {
            lobbyBoy.get = false;
            lobbyBoy.wait = true;
            return;
        }
        if (inReception && !reception.customers.Contains(customer))
        {
            lobbyBoy.get = false;
            lobbyBoy.wait = true;
            return;
        }
        if (coach && !coaches.HasCustomer(customer))
        {
            lobbyBoy.get = false;
            lobbyBoy.wait = true;
            return;
        }
        if (customer.interacted)
        {
            if (Reception.Instance.customers.Count > 0 && !lobbyBoy.customerGetter.GetCustomer())
            {
                customer = Reception.Instance.customers[0];
                navMeshAgent.SetDestination(customer.transform.position);
                return;
            }
            else if (!lobbyBoy.customerGetter.GetCustomer())
            {
                lobbyBoy.get = false;
                lobbyBoy.wait = true;
                return;
            }
        }
        if (lobbyBoy.customerGetter.GetCustomer())
        {
            lobbyBoy.get = false;
            lobbyBoy.go = true;
            return;
        }
        if (!customer.getColl.enabled)
        {
            lobbyBoy.get = false;
            lobbyBoy.wait = true;
            return;
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
        FindRoom();
    }

    private void FindRoom()
    {
        var closest = 99f;
        Room target = null;
        foreach (var room in RoomLister.Instance.rooms)
        {
            if (room.available && !room.GetCustomer())
            {
                var distance = Vector3.Distance(lobbyBoy.transform.position,
                    room.door.transform.position);
                if (distance < closest)
                {
                    target = room;
                    closest = distance;
                }
            }
        }

        if (target == null) return;

        navMeshAgent.SetDestination(target.door.transform.position);
        check = true;
        lobbyBoy.anim.SetBool("Walk", true);
    }

    public void OnExit()
    {
        check = false;
    }

    public void Tick()
    {
        if (!lobbyBoy.HasRoom)
        {
            lobbyBoy.go = false;
            lobbyBoy.wait = true;
            return;
        }
        if (check)
        {
            if (!lobbyBoy.HasRoom || !lobbyBoy.customerGetter.GetCustomer())
            {
                lobbyBoy.get = false;
                lobbyBoy.go = false;
                lobbyBoy.wait = true;
                return;
            }
            if (lobbyBoy.HasRoom && lobbyBoy.customerGetter.GetCustomer())
            {
                lobbyBoy.anim.SetBool("Walk", false);
                check = false;
                FindRoom();
                return;
            }
        }
        if (lobbyBoy.customerGetter.GetCustomer().patiance <= 0)
        {
            lobbyBoy.customerGetter.SetCustomer(null);
            lobbyBoy.get = false;
            lobbyBoy.go = false;
            lobbyBoy.wait = true;
        }
    }
}
