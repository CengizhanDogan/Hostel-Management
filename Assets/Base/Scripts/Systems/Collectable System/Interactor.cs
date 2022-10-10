using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private CustomerGetter customerGetter;

    private void Start()
    {
        customerGetter = GetComponent<CustomerGetter>();
    }
    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponentInChildren<IInteractable>();
        if(interactable != null)
        {
            interactable.Interact(customerGetter);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        IInteractable interactable = collision.collider.GetComponentInChildren<IInteractable>();
        if (interactable != null)
        {
            interactable.Interact(customerGetter);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        IExitable exitable = other.GetComponentInChildren<IExitable>();
        if (exitable != null)
        {
            exitable.Exit();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        IExitable exitable = collision.collider.GetComponentInChildren<IExitable>();
        if (exitable != null)
        {
            exitable.Exit();
        }
    }
}
