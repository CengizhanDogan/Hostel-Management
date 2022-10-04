using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private Room room;

    private void Start()
    {
        room.door = this;
        PlayerPrefs.SetInt(PlayerPrefKeys.Coin, 1000);
    }

    public void Interact(ManagerBehaviour manager)
    {
        if (room.available)
        {
            if (manager.GetCustomer() == null || room.GetCustomer() != null) return;

            var customer = manager.GetCustomer();
            customer.SetToRoom(room);
            room.SetCustomer(customer);
        }
    }
}
