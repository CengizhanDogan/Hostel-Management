using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGetter : MonoBehaviour
{
    private CustomerBehaviour customer;
    public bool isPlayer;

    public void SetCustomer(CustomerBehaviour customer)
    {
        this.customer = customer;
    }

    public CustomerBehaviour GetCustomer() { return this.customer; }
}
