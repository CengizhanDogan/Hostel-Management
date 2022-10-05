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
            while (Reception.Instance.available)
            {
                var customerClone = Instantiate(customer, transform.position, customer.gameObject.transform.rotation);
                yield return new WaitForSeconds(10f);
            }
            yield return null;
        }
    }
}
