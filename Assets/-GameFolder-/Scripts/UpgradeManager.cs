using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class UpgradeManager : MonoBehaviour, IPurchasable
{
    private int updateCount;
    private int listOrder;
    [SerializeField] private int cost;

    [SerializeField] private List<GameObject> ugradeObjects = new List<GameObject>();
    [SerializeField] private List<Renderer> renderers = new List<Renderer>();
    [SerializeField] private List<Material> materials = new List<Material>();

    [SerializeField] private bool isKitchen;
    private void Start()
    {
        PlayerPrefs.SetInt(PlayerPrefKeys.Coin, 100000);
    }
    public int GetCost()
    {
        UpdateValues();
        return cost;
    }

    public void GetPurchased()
    {
        Debug.Log("Get Purchased");
        SetMaterials();
        var removedObject = ugradeObjects[listOrder];
        Debug.Log("Get Object: " + removedObject);
        removedObject.transform.DOScale(0, 0.5f)
            .OnComplete(() => removedObject.SetActive(false));

        Debug.Log("Remove Object: " + removedObject);
        var upgradedTransform = ugradeObjects[listOrder + 1].transform;

        Debug.Log("Upgraded Object: " + removedObject);
        upgradedTransform.gameObject.SetActive(true);

        var scale = upgradedTransform.localScale; upgradedTransform.localScale = Vector3.zero;

        upgradedTransform.DOScale(scale, 0.5f).SetEase(Ease.OutBounce);

        Debug.Log("Spawn Object: " + removedObject);

        if (isKitchen)
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.KitchenLevel,
            PlayerPrefs.GetInt(PlayerPrefKeys.KitchenLevel) + 1);
        }
    }

    private void SetMaterials()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            Debug.Log("Set Materials: " + materials[listOrder] + " " + renderers[i].material);
            if (!renderers[i]) continue;
            if (renderers.Count > 2 && i == renderers.Count - 1)
            {
                renderers[i].material = materials[listOrder + 2];
            }
            else
            {
                renderers[i].material = materials[listOrder];
            }
        }
    }

    private void UpdateValues()
    {
        if (updateCount > 0)
        {
            listOrder++;
            cost *= 2;
        }
        updateCount++;
    }
}
