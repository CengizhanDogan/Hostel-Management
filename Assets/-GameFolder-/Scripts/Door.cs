using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private Room room;

    public void Interact(ManagerBehaviour manager)
    {
        if (manager.GetCustomer() == null || room.GetCustomer() != null) return;

        var customer = manager.GetCustomer();
        customer.SetToRoom(room);
        room.SetCustomer(customer);
    }
}
