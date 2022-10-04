using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private CustomerBehaviour customer;
    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(7.5f);
            Instantiate(customer, transform.position, customer.gameObject.transform.rotation);
        }
    }
}
