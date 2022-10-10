using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Collidor : MonoBehaviour, IPurchasable
{
    [SerializeField] private int cost;

    [SerializeField] private List<Transform> longWalls = new List<Transform>();
    [SerializeField] private GameObject door;

    [SerializeField] private GameObject enabler;
    public int GetCost()
    {
        return cost;
    }

    public void GetPurchased()
    {
        enabler.SetActive(true);
        transform.DOScaleZ(1, 0.75f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            foreach (Transform t in longWalls) { t.DOMoveY(-4f, 0.5f); }
            door.SetActive(true);
        });
    }
}
