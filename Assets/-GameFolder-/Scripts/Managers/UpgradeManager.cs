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

    }
    public int GetCost(PurchaseBehaviour pb)
    {
        UpdateValues();
        return cost;
    }

    public void GetPurchased()
    {
        SetMaterials();
        var removedObject = ugradeObjects[listOrder];

        removedObject.transform.DOScale(0, 0.5f)
            .OnComplete(() => removedObject.SetActive(false));


        var upgradedTransform = ugradeObjects[listOrder + 1].transform;

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
    }

    private void SetMaterials()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
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

    public bool IsPurchased()
    {
        return false;
    }

    public void SetBool(bool set)
    {

    }
}
