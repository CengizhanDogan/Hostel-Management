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

    public int GetCost()
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

        upgradedTransform.DOScale(scale, 0.5f).SetEase(Ease.OutBounce);
    }

    private void SetMaterials()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            if (i == renderers.Count - 1)
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
