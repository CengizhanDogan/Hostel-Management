using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBehaviour : Singleton<ManagerBehaviour>
{
    private CustomerBehaviour customer;

    public void SetCustomer(CustomerBehaviour customer)
    {
        this.customer = customer;
    }

    public CustomerBehaviour GetCustomer() { return this.customer; }
}
