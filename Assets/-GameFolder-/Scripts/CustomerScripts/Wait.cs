using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Wait : IState
{
    private CustomerBehaviour customerBehaviour;
    private ReceptionBehaviour receptionBehaviour;
    private Reception reception;
    private Timer timer;
    private Coaches coaches;
    private float patiance;

    public bool reorder;
    private bool timerStarted;
    private bool spawnOnce;
    private bool spawnTwice;

    public Wait(CustomerBehaviour customerBehaviour, ReceptionBehaviour receptionBehaviour, Timer timer)
    {
        this.customerBehaviour = customerBehaviour;
        this.receptionBehaviour = receptionBehaviour;
        this.timer = timer;
        patiance = customerBehaviour.patiance;
        coaches = Coaches.Instance;
        reception = Reception.Instance;
    }

    public void OnEnter() 
    {
        if (!timerStarted)
        {
            timer.StartTimer();
            timerStarted = true;
        }
        customerBehaviour.getColl.enabled = true;
    }

    public void OnExit() 
    {
        Bell.Instance.DestroyBubble();
    }

    public void Tick()
    {
        var spawnPos = customerBehaviour.timerTransform.position; spawnPos.y += 0.5f;
        customerBehaviour.patiance -= Time.deltaTime;

        if (reception.customers.IndexOf(customerBehaviour) == 0 && coaches.available)
        {
            foreach (var seat in coaches.seats)
            {
                if (seat.customerBehaviour == null)
                {
                    return;
                }
            }
        }
        if (reorder)
        {
            reorder = false;
            receptionBehaviour.end = false;
        }
        if (customerBehaviour.patiance <= 0 )
        {
            timer.StopTimer();
            customerBehaviour.exit = true;
        }
        if (customerBehaviour.patiance <= patiance / 2 && !spawnOnce)
        {
            spawnOnce = true;

            var particle = PoolingSystem.Instance.InstantiateAPS("Angry2", spawnPos);
            particle.transform.DOScale(1.25f, 5f).OnComplete(()=> PoolingSystem.Instance.DestroyAPS(particle));
        }
        if (customerBehaviour.patiance <= patiance / 4 && !spawnTwice)
        {
            spawnTwice = true;

            var particle = PoolingSystem.Instance.InstantiateAPS("Angry", spawnPos);
            particle.transform.DOScale(1.25f, 5f).OnComplete(() => PoolingSystem.Instance.DestroyAPS(particle));
        }
    }
}
