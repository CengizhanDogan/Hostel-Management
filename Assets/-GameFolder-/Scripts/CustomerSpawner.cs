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
        for (int i = 0; i < 5; i++)
        {
            var customerClone = Instantiate(customer, transform.position, customer.gameObject.transform.rotation);
            yield return new WaitForSeconds(1f);
        }
        //while (true)
        //{
            
        //}
    }
}
