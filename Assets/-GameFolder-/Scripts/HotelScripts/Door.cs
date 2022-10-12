using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private Room room;
    [SerializeField] private Transform grayArea;

    private Vector3 scale;

    [HideInInspector] public Collider coll;
    private void Start()
    {
        room.door = this;
        coll = GetComponent<Collider>();
        PlayerPrefs.SetInt(PlayerPrefKeys.Coin, 1000);
        scale = grayArea.localScale;
    }

    public void Interact(Interactor interactor)
    {
        if (room.available)
        {
            if (interactor.TryGetComponent(out CustomerGetter manager))
            {
                if (manager.GetCustomer() == null || room.GetCustomer() != null) return;

                var customer = manager.GetCustomer();
                customer.SetToRoom(room);
                room.SetCustomer(customer);
                manager.SetCustomer(null);

                grayArea.DOScale(scale + Vector3.one * 0.1f, 0.5f)
                    .OnComplete(() => grayArea.DOScale(scale, 0.5f));
            }
        }
    }
}
