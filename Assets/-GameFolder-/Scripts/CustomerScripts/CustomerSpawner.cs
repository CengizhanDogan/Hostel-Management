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
        while (true)
        {
            while (Reception.Instance.available)
            {
                int starValue = 3 * (PlayerPrefs.GetInt(PlayerPrefKeys.HotelStarLevel) / 20);
                var customerClone = Instantiate(customer, transform.position, customer.gameObject.transform.rotation);
                yield return new WaitForSeconds(spawnFrequency - starValue);
            }
            yield return null;
        }
    }
}
