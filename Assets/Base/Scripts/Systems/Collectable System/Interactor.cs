using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponentInChildren<IInteractable>();
        if(interactable != null)
        {
            interactable.Interact(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        IInteractable interactable = collision.collider.GetComponentInChildren<IInteractable>();
        if (interactable != null)
        {
            interactable.Interact(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        IExitable exitable = other.GetComponentInChildren<IExitable>();
        if (exitable != null)
        {
            if (!TryGetComponent(out CustomerGetter cg)) return;
            if (!cg.isPlayer) return;
            exitable.Exit();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        IExitable exitable = collision.collider.GetComponentInChildren<IExitable>();
        if (exitable != null)
        {
            if (!TryGetComponent(out CustomerGetter cg)) return;
            if (!cg.isPlayer) return;
            exitable.Exit();
        }
    }
}
