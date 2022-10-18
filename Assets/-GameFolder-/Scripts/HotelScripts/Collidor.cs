using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Collidor : MonoBehaviour, IPurchasable
{
    [SerializeField] private int cost;

    [SerializeField] private List<Transform> longWalls = new List<Transform>();
    [SerializeField] private List<Room> rooms = new List<Room>();
    [SerializeField] private GameObject door;

    [SerializeField] private GameObject enabler;

    private PurchaseBehaviour purchaseBehaviour;

    private void Start()
    {
        if (enabler.activeSelf)
        {
            purchaseBehaviour.Loaded();
            GetPurchased();
        }
    }
    public int GetCost(PurchaseBehaviour pb)
    {
        purchaseBehaviour = pb;
        return cost;
    }

    public void GetPurchased()
    {
        enabler.SetActive(true);
        enabler.transform.DOScaleZ(1, 0.75f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            door.SetActive(true);
            foreach (Room room in rooms)
                if (room.available) room.GetPurchased();
        });
        foreach (Transform t in longWalls) { t.DOMoveY(-4f, 0.5f); }
    }

    public bool IsPurchased()
    {
        return enabler.activeSelf;
    }

    public void SetBool(bool set)
    {
        enabler.SetActive(set);
    }
}
