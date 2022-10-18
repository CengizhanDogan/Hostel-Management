using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using GameAnalyticsSDK;

public class Room : MonoBehaviour, IPurchasable, ITimer
{
    private CustomerBehaviour roomCustomer;

    public int roomValue;

    public Door door;
    public UpgradeManager upgradeManager;

    [SerializeField] private Timer timer;
    public bool available;
    [SerializeField] private Rigidbody doorRb;
    [SerializeField] private List<Transform> longWalls = new List<Transform>();

    public Transform sitTransform;
    public Transform sleepTransform;

    public Cloud cloud;
    [SerializeField] private Order order;
    private PurchaseBehaviour purchaseBehaviour;
    public Collidor collidor;
    [SerializeField] private Renderer shadow;

    private void Start()
    {
        upgradeManager = GetComponentInChildren<UpgradeManager>();
        order.room = this;
        if (available && !collidor)
        {
            if (purchaseBehaviour) purchaseBehaviour.Loaded();
            GetPurchased();
        }
        SetShadow(false);
    }
    public void SetCustomer(CustomerBehaviour customer)
    {
        if (!customer) timer.StopTimer();
        roomCustomer = customer;
        door.coll.enabled = !customer;
        if (customer) timer.StartTimer();
        door.SetGrayArea(customer);
        SetShadow(customer);
    }

    private void SetShadow(bool set)
    {
        float alphaValue = 0;
        if (set) alphaValue = 60;

        float currentValue = shadow.material.color.a;
        Color32 currentColor = shadow.material.color;

        DOTween.To(() => currentValue, x => currentValue = x, alphaValue, 0.5f).OnUpdate(() => 
            { 
                currentColor.a = (byte)currentValue;
                shadow.material.color = currentColor;
            });
    }

    public CustomerBehaviour GetCustomer() { return roomCustomer; }

    public int GetCost(PurchaseBehaviour pb)
    {
        purchaseBehaviour = pb;
        return roomValue;
    }

    public void GetPurchased()
    {
        doorRb.isKinematic = false;

        available = true;
        transform.DOMoveY(0, .5f).SetEase(Ease.Flash).OnComplete(() =>
        {
            var spawnPos = transform.position; spawnPos.y += 0.5f;
            var particle = PoolingSystem.Instance.InstantiateAPS("Slam", spawnPos);
            particle.transform.eulerAngles = Vector3.right * 90f;
            door.coll.enabled = true;
            doorRb.isKinematic = false;
            particle.transform.DOScale(2f, 5f).OnComplete(() => PoolingSystem.Instance.DestroyAPS(particle));
            if (PlayerPrefs.GetInt(PlayerPrefKeys.CustomerTutorial) == 0)
            {
                PlayerPrefs.SetInt(PlayerPrefKeys.CustomerTutorial, 1);
                TutorialManager.Instance.CustomerTutorial(door.transform);
            }
        });
        foreach (var longWall in longWalls)
        {
            longWall.DOMoveY(-4, 1f).SetEase(Ease.OutBack).OnComplete(() => Destroy(longWall.gameObject));
        }
        if (purchaseBehaviour)
        {
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "Room", 1, "element", "0003");
        }
    }

    public float Time()
    {
        var time = 0f;
        if (roomCustomer) time = roomCustomer.roomTime;
        return time;
    }

    public Color TargetColor()
    {
        return Color.green;
    }

    public void SetOrder(bool set)
    {
        order.SetBubbles(set);
    }

    public bool IsPurchased()
    {
        return available;
    }

    public void SetBool(bool set)
    {
        available = set;
    }
}
