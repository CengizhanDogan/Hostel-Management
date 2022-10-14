using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private CustomerBehaviour customer;
    [SerializeField] private float spawnFrequency;
    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        var reception = Reception.Instance;
        while (true)
        {
            while (reception.available && reception.customers.Count < 8)
            {
                int starValue = 5 * (PlayerPrefs.GetInt(PlayerPrefKeys.HotelStarLevel) / 20);
                if (starValue >= 15) starValue = 14;
                var customerClone = Instantiate(customer, transform.position, customer.gameObject.transform.rotation);
                yield return new WaitForSeconds(spawnFrequency - starValue);
            }
            yield return null;
        }
    }
}
