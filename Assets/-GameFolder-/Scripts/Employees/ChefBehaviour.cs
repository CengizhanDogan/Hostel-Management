using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class ChefBehaviour : MonoBehaviour, IPurchasable
{
    private StateMachine stateMachine;

    public bool get;
    public bool wait;
    public bool go;
    private bool purchased;

    public Kitchen kitchen;

    public Animator anim;

    [HideInInspector] public FoodDelivery delivery;
    private PurchaseBehaviour purchaseBehaviour;

    void Awake()
    {
        delivery = GetComponent<FoodDelivery>();
        var startPos = transform.position;
        var navMeshAgent = GetComponent<NavMeshAgent>();

        stateMachine = new StateMachine();

        var waitBehaviour = new ChefWait(this, navMeshAgent, startPos);
        var getFood = new GetFood(this, navMeshAgent);
        var serveFood = new ServeFood(this, navMeshAgent);
        var takeOutThrash = new TakeOutTrash(this, navMeshAgent);

        At(waitBehaviour, getFood, DoGet());
        At(getFood, serveFood, DoGo());
        At(serveFood, waitBehaviour, DoWait());
        At(serveFood, takeOutThrash, DoTrash());
        At(takeOutThrash, waitBehaviour, DoWait());

        stateMachine.SetState(waitBehaviour);

        void At(IState to, IState from, Func<bool> predicate) => stateMachine.AddTransition(to, from, predicate);

        Func<bool> DoGet() => () => (get && purchased && !delivery.GetFood()) && navMeshAgent.enabled;
        Func<bool> DoGo() => () => (go && delivery.GetFood()) && navMeshAgent.enabled;
        Func<bool> DoWait() => () => ((!go || !get) && !delivery.GetFood()) && navMeshAgent.enabled;
        Func<bool> DoTrash() => () => ((!go || !get) && delivery.GetFood()) && navMeshAgent.enabled;
    }

    private void Start()
    {
        if (purchased)
        {
            purchaseBehaviour.Loaded();
            GetPurchased();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Fridge fridge))
        {
            go = true;
        }
    }
    void Update() { stateMachine.Tick(); }

    public int GetCost(PurchaseBehaviour pb)
    {
        purchaseBehaviour = pb;
        return 500;
    }

    public void GetPurchased()
    {
        transform.DOScale(0.5f, 0.5f).SetEase(Ease.OutBounce)
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
}

public class GetFood : IState
{
    private ChefBehaviour chef;
    private NavMeshAgent navMeshAgent;
    public GetFood(ChefBehaviour chef, NavMeshAgent navMeshAgent)
    {
        this.chef = chef;
        this.navMeshAgent = navMeshAgent;
    }

    public void OnEnter()
    {
        chef.anim.SetBool("Walk", true);
        navMeshAgent.SetDestination(chef.kitchen.fridges
            [PlayerPrefs.GetInt(PlayerPrefKeys.KitchenLevel)].transform.position);
    }

    public void OnExit()
    {

    }

    public void Tick()
    {

    }
}
public class ChefWait : IState
{
    private ChefBehaviour chef;
    private NavMeshAgent navMeshAgent;

    private bool turn;
    private bool check;
    private bool doOnce;

    private Vector3 startPos;

    public ChefWait(ChefBehaviour chef, NavMeshAgent navMeshAgent, Vector3 startPos)
    {
        this.chef = chef;
        this.navMeshAgent = navMeshAgent;
        this.startPos = startPos;
    }

    public void OnEnter()
    {
        chef.anim.SetBool("Walk", true);
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
        if (navMeshAgent.hasPath && !check)
        {
            check = true;
            turn = true;
        }
        if (!navMeshAgent.hasPath && turn)
        {
            turn = false;
            chef.anim.SetBool("Walk", false);
            chef.transform.DORotate(Vector3.up * 180, 0.5f);
        }
        foreach (var room in RoomLister.Instance.rooms)
        {
            if (room.available && room.GetCustomer())
            {
                if (!room.GetCustomer().GetComponent<FoodOrder>().HasOrder) continue;
                chef.get = true;
                return;
            }
        }
    }
}

public class ServeFood : IState
{
    private ChefBehaviour chef;
    private NavMeshAgent navMeshAgent;

    private bool check;
    private FoodOrder foodOrder;
    public ServeFood(ChefBehaviour chef, NavMeshAgent navMeshAgent)
    {
        this.chef = chef;
        this.navMeshAgent = navMeshAgent;
    }
    public void OnEnter()
    {
        Check();

    }
    private void Check()
    {
        if (!chef.delivery.GetFood()) return;

        var closest = 99f;
        Room target = null;
        foreach (var room in RoomLister.Instance.rooms)
        {
            if (room.available && room.GetCustomer())
            {
                var order = room.GetCustomer().GetComponent<FoodOrder>();
                if (!order.HasOrder) continue;

                var distance = Vector3.Distance(chef.transform.position,
                    room.door.transform.position);
                if (distance < closest)
                {
                    foodOrder = order;
                    target = room;
                    closest = distance;
                }
            }
        }

        if (!target)
        {
            chef.get = false;
            chef.go = false;
            return;
        }
        navMeshAgent.SetDestination(target.door.transform.position);
        check = true;
        chef.anim.SetBool("Tray", true);

    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        if (!chef.delivery.GetFood())
        {
            chef.get = false;
            chef.go = false;
            
            chef.anim.SetBool("Tray", false);
            return;
        }
        else if (check)
        {
            if ((!navMeshAgent.hasPath || !foodOrder.HasOrder) && chef.delivery.GetFood())
            {
                check = false;
                Check();
                return;
            }
        }
    }
}
public class TakeOutTrash : IState
{
    private ChefBehaviour chef;
    private NavMeshAgent navMeshAgent;

    public TakeOutTrash(ChefBehaviour chef, NavMeshAgent navMeshAgent)
    {
        this.chef = chef;
        this.navMeshAgent = navMeshAgent;
    }
    public void OnEnter()
    {
        navMeshAgent.SetDestination(chef.kitchen.trashCan.position);
    }

    public void OnExit()
    {
        chef.anim.SetBool("Tray", false);
    }

    public void Tick()
    {
    }
}
