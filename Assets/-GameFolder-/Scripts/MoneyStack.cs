using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyStack : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt(PlayerPrefKeys.Tutorial) > 0) gameObject.SetActive(false);

        Invoke("GiveMoney", 0.1f);
    }

    private void GiveMoney()
    {
        if (PlayerPrefs.GetInt(PlayerPrefKeys.Tutorial) > 0 &&
           PurchaseManager.Instance.ButtonOrder < 3)
        {
            if (PlayerPrefs.GetInt(PlayerPrefKeys.Coin) < 10 && PurchaseManager.Instance.ButtonOrder < 3)
            {
                PlayerPrefs.SetInt(PlayerPrefKeys.Coin, 10);
            }
            if (PlayerPrefs.GetInt(PlayerPrefKeys.Coin) > 20 && PurchaseManager.Instance.ButtonOrder < 2)
            {
                PlayerPrefs.SetInt(PlayerPrefKeys.Coin, 20);
            }
            if (PlayerPrefs.GetInt(PlayerPrefKeys.Coin) < 50 && PurchaseManager.Instance.ButtonOrder < 1)
            {
                PlayerPrefs.SetInt(PlayerPrefKeys.Coin, 50);
            }
        }

        PlayerPrefs.SetInt(PlayerPrefKeys.Tutorial, 1);
    }
}
