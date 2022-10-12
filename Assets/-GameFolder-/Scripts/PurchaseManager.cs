using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseManager : Singleton<PurchaseManager>
{
    private List<IPurchasable> purchasables = new List<IPurchasable>();

    private int buttonOrder;

    public void AddPurchasable(IPurchasable purchasable)
    {
        purchasables.Add(purchasable);
    }

    public void IncreaseOrder()
    {
        buttonOrder++;
        EventManager.OnPurchaseEvent.Invoke(buttonOrder);
    }
}
