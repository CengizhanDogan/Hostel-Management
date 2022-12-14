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

    [HideInInspector] public Transform arrow;
    private void Start()
    {
        room.door = this;
        coll = GetComponent<Collider>();
        scale = grayArea.localScale;
    }

    public void Interact(Interactor interactor)
    {
        if (room.available)
        {
            if (interactor.TryGetComponent(out CustomerGetter manager))
            {
                if (manager.GetCustomer() == null || room.GetCustomer() != null) return;

                if(arrow) Destroy(arrow.gameObject);
                var customer = manager.GetCustomer();
                customer.SetToRoom(room);
                room.SetCustomer(customer);
                manager.SetCustomer(null);
            }
        }
    }

    public void SetGrayArea(bool set)
    {
        if (set) grayArea.DOScale(0, 0.5f);
        else grayArea.DOScale(scale, 0.5f);
    }
}
