using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PurchaseManager : Singleton<PurchaseManager>
{
    private List<IPurchasable> purchasables = new List<IPurchasable>();
    [SerializeField] private List<UpgradeManager> upgrades = new List<UpgradeManager>();

    private int buttonOrder;

    private SaveManager saveManager;

    private Reception reception;

    private void Awake()
    {
        saveManager = SaveManager.Instance;
        reception = Reception.Instance;

        SetPurchasables();
        SetUpgrades();
    }
    private void Update()
    {
        if (reception.available) return;
        if (PlayerPrefs.GetInt(PlayerPrefKeys.Coin) >= 45)
            EventManager.OnPurchaseEvent.Invoke(0, false);
    }
    private void SetUpgrades()
    {
        var upgradeArray =
           FindObjectsOfType<UpgradeManager>();

        foreach (var upgrade in upgradeArray)
        {
            upgrades.Add(upgrade);
        }
        foreach (UpgradeManager upgrade in upgrades)
        {
            if (upgrades.IndexOf(upgrade) >= saveManager.saveData.
                upgraded.Count) return;
            upgrade.listOrder = saveManager.saveData.upgraded[upgrades.IndexOf(upgrade)];
        }
    }

    private void SetPurchasables()
    {
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
    public void SaveList()
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
        foreach (UpgradeManager upgrade in upgrades)
        {
            if (upgrades.IndexOf(upgrade) >=
                saveManager.saveData.upgraded.Count)
            {
                saveManager.saveData.upgraded.Add(upgrade.listOrder);
            }
            else
            {
                saveManager.saveData.upgraded
                    [upgrades.IndexOf(upgrade)] = upgrade.listOrder;
            }
        }

        saveManager.Save();
    }
    public void IncreaseOrder(bool isLoaded)
    {
        buttonOrder++;
        if (!isLoaded) SaveList();
        EventManager.OnPurchaseEvent.Invoke(buttonOrder, isLoaded);
    }
    public int ButtonOrder { get => buttonOrder; }
}
