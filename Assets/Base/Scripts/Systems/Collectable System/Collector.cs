using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ICollectable collectable = other.GetComponentInChildren<ICollectable>();
        if(collectable != null)
        {
            collectable.Collect(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ICollectable collectable = collision.collider.GetComponentInChildren<ICollectable>();
        if (collectable != null)
        {
            collectable.Collect(this);
        }
    }
}
