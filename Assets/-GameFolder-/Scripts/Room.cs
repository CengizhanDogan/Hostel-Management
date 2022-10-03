using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private CustomerBehaviour roomCustomer;

    public int roomValue;

    public bool available;
    public void SetCustomer(CustomerBehaviour customer) { roomCustomer = customer; }
    public CustomerBehaviour GetCustomer() { return roomCustomer; }
}
