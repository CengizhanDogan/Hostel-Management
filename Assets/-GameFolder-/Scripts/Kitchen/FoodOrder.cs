using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOrder : MonoBehaviour
{
    private CustomerBehaviour customer;
    private float roomTime;

    public bool HasOrder { get; private set; }
    private bool MakeOrder
    {
        get
        {
            return customer.roomTime < roomTime
                && customer.roomTime > roomTime / 3 
                && Random.value < 0.001f
                && Kitchen.Instance.available;
        }
    }
    private bool doOnce;

    private void Start()
    {
        customer = GetComponent<CustomerBehaviour>();
        roomTime = customer.roomTime;
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
