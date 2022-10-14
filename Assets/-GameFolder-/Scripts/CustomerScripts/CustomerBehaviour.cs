using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class CustomerBehaviour : MonoBehaviour, IInteractable, ITimer
{
    public CustomerAnimation customerAnimation;
    private StateMachine stateMachine;

    [HideInInspector] public RoomBehaviour roomBehaviour;
    [HideInInspector] public Room room;

    private CustomerFollow customerFollow;
    private Wait wait;
    public Collider getColl;

    public float patiance;
    public float roomTime;
    private float moneyCount;

    public bool interacted;
    [HideInInspector] public bool exit;
    [HideInInspector] public bool giveMoney;

    [SerializeField] private Timer timer;
    public Transform timerTransform;
    public Transform areaTransform;

    private void Awake()
    {
        moneyCount = roomTime;
        timer = Instantiate(timer, timerTransform.position, timer.transform.rotation);
        timer.timerObject = gameObject;
        timer.follow = true;
        timer.followTransform = timerTransform;

        customerAnimation = GetComponent<CustomerAnimation>();
        var navMeshAgent = GetComponent<NavMeshAgent>();

        roomBehaviour = gameObject.AddComponent<RoomBehaviour>();
        roomBehaviour.SetCustomerBehaviour(this);

        stateMachine = new StateMachine();

        var receptionBehavior = new ReceptionBehaviour(navMeshAgent, this);
        wait = new Wait(this, receptionBehavior, timer);
        customerFollow = new CustomerFollow(navMeshAgent, this, timer);
        var exitHotel = new ExitHotel(transform.position, navMeshAgent, transform, this);

        At(receptionBehavior, wait, ReachedReception());
        At(wait, receptionBehavior, Reorder());
        At(wait, customerFollow, Getted());
        At(customerFollow, roomBehaviour, HasRoom());

        stateMachine.AddAnyTransition(exitHotel, ExitHotel());

        stateMachine.SetState(receptionBehavior);

        void At(IState to, IState from, Func<bool> predicate) => stateMachine.AddTransition(to, from, predicate);

        Func<bool> ReachedReception() => () => receptionBehavior.end;
        Func<bool> Reorder() => () => !receptionBehavior.end;
        Func<bool> Getted() => () => interacted;
        Func<bool> HasRoom() => () => room != null;
        Func<bool> ExitHotel() => () => exit;
    }

    private void Update() => stateMachine.Tick();

    public void Interact(Interactor interactor)
    {
        var manager = interactor.GetComponent<CustomerGetter>();

        if (manager.GetCustomer()) return;

        customerFollow.manager = manager;
        interacted = true;
        getColl.enabled = false;
        manager.SetCustomer(this);
        Reception.Instance.RemoveCustomer(this);
        foreach (var customer in Reception.Instance.customers)
        {
            customer.SetReorder();
        }
        areaTransform.DOScale(0, 0.6f).SetEase(Ease.InBack)
            .OnComplete(()=> Destroy(areaTransform.gameObject));
    }

    public void SetToRoom(Room room)
    {
        roomBehaviour.SetRoom(room);
        this.room = room;
    }

    public void SetReorder()
    {
        wait.reorder = true;
    }

    public void DestroyCustomer()
    {
        Destroy(gameObject);
    }

    public void SpawnMoney()
    {
        for (int i = 0; i < moneyCount; i++)
        {
            Vector3 spawnPos = (UnityEngine.Random.insideUnitCircle * 1f);
            spawnPos.z = spawnPos.y; spawnPos.y = 0f;
            spawnPos += transform.position; spawnPos.y += 1f;

            var cash = PoolingSystem.Instance.InstantiateAPS("Cash", spawnPos);
            cash.GetComponent<Money>().SetColliders(true);

            cash.transform.DOJump(spawnPos, 2.5f, 1, 1f).SetId("Jump");
        }
    }

    public float Time()
    {
        return patiance;
    }
    public Color TargetColor()
    {
        return Color.red;
    }
}

public class ExitHotel : IState
{
    private Vector3 pos;
    private NavMeshAgent navMeshAgent;
    private Transform transform;
    private CustomerBehaviour customerBehaviour;

    private bool canExit;
    private bool giveMoney;
    private bool exited;

    private GameObject particle;
    public ExitHotel(Vector3 pos, NavMeshAgent navMeshAgent, Transform transform,
        CustomerBehaviour customerBehaviour)
    {
        this.pos = pos;
        this.navMeshAgent = navMeshAgent;
        this.transform = transform;
        this.customerBehaviour = customerBehaviour;
    }

    public void Tick()
    {
        var starValue = PlayerPrefs.GetInt(PlayerPrefKeys.HotelStarLevel);
        if (customerBehaviour.giveMoney)
        {
            customerBehaviour.room.cloud.SetCloud(false);
            navMeshAgent.SetDestination(Reception.Instance.WaitPos(-1));
            if (!giveMoney && navMeshAgent.hasPath)
            {
                giveMoney = true;
                customerBehaviour.giveMoney = false;
            }
            else return;
        }
        else canExit = true;


        if (giveMoney)
            if (!navMeshAgent.hasPath)
            {
                giveMoney = false;
                customerBehaviour.SpawnMoney();

                if(starValue < 100)
                PlayerPrefs.SetInt(PlayerPrefKeys.HotelStarLevel,
                    starValue + 5);
                else PlayerPrefs.SetInt(PlayerPrefKeys.HotelStarLevel,
                    100);

                particle = PoolingSystem.Instance.InstantiateAPS("Happy", customerBehaviour.timerTransform.position);

                particle.transform.SetParent(customerBehaviour.transform);
            }
        if (canExit && !navMeshAgent.hasPath)
        {
            navMeshAgent.SetDestination(pos); 
            if (Reception.Instance.customers.Contains(customerBehaviour))
            {
                if (starValue > 5)
                PlayerPrefs.SetInt(PlayerPrefKeys.HotelStarLevel,
                        starValue - 5);
                else if (starValue < 0)
                    PlayerPrefs.SetInt(PlayerPrefKeys.HotelStarLevel,
                        0);

                particle = PoolingSystem.Instance.InstantiateAPS("Angry", customerBehaviour.timerTransform.position);

                particle.transform.SetParent(customerBehaviour.transform);

                Reception.Instance.RemoveCustomer(customerBehaviour);
                foreach (var customer in Reception.Instance.customers)
                {
                    customer.SetReorder();
                }
            }
            EventManager.OnCustomerLeave.Invoke();
            canExit = false;
            return;
        }
        if (!exited && Vector3.Distance(transform.position, pos) < 0.5f)
        {
            exited = true;

            PoolingSystem.Instance.DestroyAPS(particle);

            transform.DOScale(0, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                customerBehaviour.DestroyCustomer();
            });
        }
    }

    public void OnEnter()
    {
        if (customerBehaviour.room)
        {
            customerBehaviour.room.SetCustomer(null);
            customerBehaviour.room.SetOrder(false);
        }
        customerBehaviour.roomBehaviour.doSleep = false;
        navMeshAgent.enabled = true;
        customerBehaviour.customerAnimation.SetSleep(false);
        customerBehaviour.customerAnimation.SetWalk(true);
    }

    public void OnExit() { }
}
