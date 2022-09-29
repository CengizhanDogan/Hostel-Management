using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private CustomerBehaviour roomCustomer;

    public void SetCustomer(CustomerBehaviour customer) { roomCustomer = customer; }
    public CustomerBehaviour GetCustomer() { return roomCustomer; }
}
