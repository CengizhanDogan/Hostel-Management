using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private CustomerBehaviour customer;
    [SerializeField] private float spawnFrequency;
    private WaitForSeconds wait;
    void Start()
    {
        StartCoroutine(Spawn());
        wait = new WaitForSeconds(spawnFrequency);
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            while (Reception.Instance.available)
            {
                var customerClone = Instantiate(customer, transform.position, customer.gameObject.transform.rotation);
                yield return wait;
            }
            yield return null;
        }
    }
}
