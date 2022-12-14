using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class CoachBehaviour : IState
{
    private CustomerBehaviour customer;
    private Reception reception;
    private NavMeshAgent nav;
    private CoachSeat seat;
    private Timer timer;
    private bool animate;
    private Vector3 startPos;
    private bool spawnOnce;
    private bool spawnTwice;
    private float patiance;

    public CoachBehaviour(CustomerBehaviour customer, NavMeshAgent nav, Timer timer)
    {
        this.customer = customer;
        this.nav = nav;
        this.timer = timer;
        patiance = customer.patiance;
        reception = Reception.Instance;
    }
    public void OnEnter()
    {
        customer.patiance = 100;
        foreach (var customer in Reception.Instance.customers)
        {
            customer.SetReorder();
        }
        timer.StopTimer();
        seat = Coaches.Instance.EmptySeat();
        customer.customerAnimation.SetWalk(true);

        seat.customerBehaviour = customer;
        reception.customers.Remove(customer);
        nav.SetDestination(seat.seatTransform.position); 
    }

    public void OnExit()
    {
        customer.transform.DOKill();
        animate = false;
        //customer.transform.position = startPos;
        nav.enabled = true;
        customer.customerAnimation.SetSit(false);
        customer.customerAnimation.SetWalk(true);
        seat.customerBehaviour = null;
    }

    public void Tick()
    {
        var spawnPos = customer.timerTransform.position; spawnPos.y += 0.5f;

        if (nav.hasPath) animate = true;
        if (!nav.hasPath && animate)
        {
            customer.customerAnimation.SetWalk(false);
            animate = false;
            startPos = customer.transform.position;
            nav.enabled = false;
            customer.transform.DORotate(seat.seatTransform.eulerAngles, 0.5f);
            customer.transform.DOMove(seat.seatTransform.position, 0.5f).OnComplete(() =>
            {
                customer.customerAnimation.SetSit(true);
                customer.getColl.enabled = true;
                customer.areaTransform.DOScale(0.5f, 0.6f).SetEase(Ease.OutBack);
            });
        }
    }
}
