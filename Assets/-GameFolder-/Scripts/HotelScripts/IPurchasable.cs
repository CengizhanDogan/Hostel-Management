using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPurchasable
{
    public int GetCost(PurchaseBehaviour pb);
    public void GetPurchased();
    public bool IsPurchased();
    public void SetBool(bool set);
}
