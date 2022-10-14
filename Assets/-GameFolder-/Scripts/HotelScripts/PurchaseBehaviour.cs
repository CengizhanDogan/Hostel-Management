using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PurchaseBehaviour : MonoBehaviour, IInteractable, IExitable
{
    private bool exited;
    [SerializeField] private bool isUpgrade;
    private bool activated;

    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Transform line;
    [SerializeField] private GameObject purchaseObject;

    private PurchaseManager purchaseManager;

    private IPurchasable purchasable;

    private int roomValue;
    private int loopValue;

    private Vector3 scale;

    [SerializeField] private int enableOrder;

    private void OnEnable()
    {
        EventManager.OnPurchaseEvent.AddListener(CheckEnable);
    }
    private void OnDisable()
    {
        EventManager.OnPurchaseEvent.RemoveListener(CheckEnable);
    }

    private void Awake()
    {
        purchaseManager = PurchaseManager.Instance;
        if (enableOrder != 0) Disable();
        else activated = true;

        purchasable = purchaseObject.GetComponent<IPurchasable>();
        roomValue = purchasable.GetCost(this);
        loopValue = roomValue;
        textMesh.text = roomValue.ToString();
        var scale = line.localScale + Vector3.one * 0.1f;
        line.DOScale(scale, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    private void Disable()
    {
        scale = transform.localScale;

        transform.localScale = Vector3.zero;
    }
    private void CheckEnable(int order, bool isLoaded)
    {
        if (enableOrder == 0) activated = true;
        if (enableOrder <= order && !activated)
        {
            if (enableOrder == 9)
                if (!purchaseObject.
                    GetComponent<ChefBehaviour>()
                    .kitchen.available) return;

            activated = true;
            transform.DOScale(scale, 0.5f).SetEase(Ease.OutBack);
            if (!isLoaded) EventManager.OnTutorialEvent.Invoke(transform);
        }
    }
    public void Interact(Interactor interactor)
    {
        var manager = interactor.GetComponent<CustomerGetter>();

        if (!manager.isPlayer) return;
        exited = false;
        StartCoroutine(SpendMoneyToRoom(manager));
    }

    private IEnumerator SpendMoneyToRoom(CustomerGetter manager)
    {
        var audioManager = AudioManager.Instance;
        while (!exited)
        {
            if (PlayerPrefs.GetInt(PlayerPrefKeys.Coin) > 0 && roomValue > 0)
            {
                PlayerPrefs.SetInt(PlayerPrefKeys.Coin, PlayerPrefs.GetInt(PlayerPrefKeys.Coin) - 1);
                roomValue -= 1;
                textMesh.text = roomValue.ToString();
                if (roomValue > 5) audioManager.PlaySound(audioManager.MoneySound, 0.1f);
                var spawnPos = manager.transform.position; spawnPos.y += 1;
                var cash = PoolingSystem.Instance.InstantiateAPS("Cash", spawnPos);
                cash.GetComponent<Money>().SetColliders(false);

                cash.transform.DOMove(transform.position, 0.5f)
                    .OnComplete(() => PoolingSystem.Instance.DestroyAPS(cash));

                foreach (var item in FindObjectOfType<LevelPanel>().inGameCoinTexts)
                {
                    item.text = PlayerPrefs.GetInt(PlayerPrefKeys.Coin).ToString();
                }
            }
            if (roomValue <= 0)
            {
                purchasable.GetPurchased();
                if (!isUpgrade)
                {
                    purchaseManager.IncreaseOrder(isLoaded: false);
                    audioManager.PurchaseSound();
                }
                else
                {
                    audioManager.UpgradeSound();
                }
                gameObject.SetActive(false);
                yield break;
            }
            yield return new WaitForSeconds(1f / loopValue);
        }
    }
    public void Exit()
    {
        exited = true;
    }

    public void Loaded()
    {
        PurchaseManager.Instance.IncreaseOrder(isLoaded: true);
        Destroy(gameObject);
    }
}
