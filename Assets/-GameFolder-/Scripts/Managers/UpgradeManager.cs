using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class UpgradeManager : MonoBehaviour, IPurchasable
{
    private int updateCount;
    public int listOrder;
    [SerializeField] private int cost;

    [SerializeField] private List<GameObject> ugradeObjects = new List<GameObject>();
    [SerializeField] private List<Renderer> renderers = new List<Renderer>();
    [SerializeField] private List<Material> materials = new List<Material>();

    [SerializeField] private bool isKitchen;
    private PurchaseBehaviour purchaseBehaviour;
    private bool load;
    private void Start()
    {
        if (listOrder > 0)
        {
            Destroy(purchaseBehaviour.gameObject);
            load = true;
            GetPurchased();
        }
    }
    public int GetCost(PurchaseBehaviour pb)
    {
        purchaseBehaviour = pb;
        UpdateValues();
        return cost;
    }

    public void GetPurchased()
    {
        if (listOrder < 2 && !load) listOrder++;

        load = false;

        SetMaterials();

        var removedObject = ugradeObjects[listOrder - 1];

        for (int i = 0; i < listOrder; i++)
        {
            if (ugradeObjects[i].activeSelf)
            {
                removedObject = ugradeObjects[i];
            }
        }

        removedObject.transform.DOScale(0, 0.5f).OnComplete(() =>
            {
                removedObject.SetActive(false);
            });


        var upgradedTransform = ugradeObjects[listOrder].transform;

        upgradedTransform.gameObject.SetActive(true);

        var scale = upgradedTransform.localScale; upgradedTransform.localScale = Vector3.zero;
        var spawnPos = upgradedTransform.position; spawnPos.y += 1.5f;
        var particle = PoolingSystem.Instance.InstantiateAPS("Sparkle", spawnPos);

        upgradedTransform.DOScale(scale, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                particle.transform.DOScale(2f, 5f).OnComplete(() => PoolingSystem.Instance.DestroyAPS(particle));
            });

        if (isKitchen)
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.KitchenLevel,
            PlayerPrefs.GetInt(PlayerPrefKeys.KitchenLevel) + 1);
        }

        PurchaseManager.Instance.SaveList();
    }

    private void SetMaterials()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            if (!renderers[i]) continue;
            if (renderers.Count > 2 && i == renderers.Count - 1)
            {
                renderers[i].material = materials[listOrder + 1];
            }
            else
            {
                renderers[i].material = materials[listOrder - 1];
            }
        }
    }

    private void UpdateValues()
    {
        if (listOrder > 0)
        {
            cost *= 2;
        }
    }

    public bool IsPurchased()
    {
        return false;
    }

    public void SetBool(bool set)
    {

    }
}
