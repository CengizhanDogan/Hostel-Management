using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOrder : MonoBehaviour
{
    private CustomerBehaviour customer;
    private float roomTime;
    private bool doOnce;
    private float foodChance;
    public bool HasOrder { get; private set; }
    private bool MakeOrder
    {
        get
        {
            return customer.roomTime < roomTime
                && foodChance < 0.5f
                && Kitchen.Instance.available;
        }
    }

    private void Start()
    {
        customer = GetComponent<CustomerBehaviour>();
        roomTime = customer.roomTime;
        foodChance = Random.value;
    }

    private void Update()
    {
        if (!MakeOrder || doOnce) return;
        
        doOnce = true;

        HasOrder = true;
        customer.room.SetOrder(true);
    }

    public void OrderDone()
    {
        HasOrder = false;
        customer.hadFood = true;
    }
}
