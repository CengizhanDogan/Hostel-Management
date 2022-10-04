using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PurchaseBehaviour : MonoBehaviour, IInteractable, IExitable
{
    private bool exited;

    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private Transform line;
    [SerializeField] private GameObject roomObject;

    private IPurchasable room;

    private int roomValue;

    private void Start()
    {
        room = roomObject.GetComponent<IPurchasable>();
        room.GetCost(out var cost);
        roomValue = cost;
        textMesh.text = roomValue.ToString();
        var scale = line.localScale + Vector3.one * 0.1f;
        line.DOScale(scale, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    public void Interact(ManagerBehaviour manager)
    {
        exited = false;
        StartCoroutine(SpendMoneyToRoom(manager));
    }
    private IEnumerator SpendMoneyToRoom(ManagerBehaviour manager)
    {
        while (!exited)
        {
            if (PlayerPrefs.GetInt(PlayerPrefKeys.Coin) > 0 && roomValue > 0)
            {
                PlayerPrefs.SetInt(PlayerPrefKeys.Coin, PlayerPrefs.GetInt(PlayerPrefKeys.Coin) - 1);
                roomValue -= 1;
                textMesh.text = roomValue.ToString();

                var spawnPos = manager.transform.position; spawnPos.y += 1;
                var cash = PoolingSystem.Instance.InstantiateAPS("Cash", spawnPos);

                cash.transform.DOMove(transform.position, 0.5f)
                    .OnComplete(() => PoolingSystem.Instance.DestroyAPS(cash));

                foreach (var item in FindObjectOfType<LevelPanel>().inGameCoinTexts)
                {
                    item.text = PlayerPrefs.GetInt(PlayerPrefKeys.Coin).ToString();
                }
            }
            if (roomValue <= 0)
            {
                room.GetPurchased();
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void Exit()
    {
        exited = true;
    }
}
