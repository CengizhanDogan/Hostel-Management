using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PurchaseManager : Singleton<PurchaseManager>
{
    private List<IPurchasable> purchasables = new List<IPurchasable>();

    private int buttonOrder;

    private SaveManager saveManager;

    private void Awake()
    {
        saveManager = SaveManager.Instance;

        var purchasableArray =
            FindObjectsOfType<MonoBehaviour>().OfType<IPurchasable>();

        foreach (var purchaseable in purchasableArray)
        {
            purchasables.Add(purchaseable);
        }
        foreach (IPurchasable purchase in purchasables)
        {
            if (purchasables.IndexOf(purchase) >= saveManager.saveData.
                purchased.Count) return;
            purchase.SetBool(saveManager.saveData.
                purchased[purchasables.IndexOf(purchase)]);
        }
    }
    private void OnApplicationQuit()
    {
        SaveList();
    }
    private void SaveList()
    {
        foreach (IPurchasable purchase in purchasables)
        {
            if (purchasables.IndexOf(purchase) >=
                saveManager.saveData.purchased.Count)
            {
                saveManager.saveData.purchased.Add(purchase.IsPurchased());
            }
            else
            {
                saveManager.saveData.purchased
                    [purchasables.IndexOf(purchase)] = purchase.IsPurchased();
            }
        }

        saveManager.Save();
    }
    public void IncreaseOrder(bool isLoaded)
    {
        buttonOrder++;
        EventManager.OnPurchaseEvent.Invoke(buttonOrder, isLoaded);
    }
}
