using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyPositioner : Singleton<MoneyPositioner>, IInteractable, IExitable
{
    private List<Money> moneyList = new List<Money>();

    [SerializeField] private float xSpace;
    [SerializeField] private float zSpace;
    [SerializeField] private float ySpace;

    private float zOrder;
    private float yOrder;

    private bool exit;
    public void PositionMoney(Money money)
    {
        money.transform.SetParent(transform);
        moneyList.Add(money);
        var pos = Vector3.zero;

        if (moneyList.Count % 8 == 0) { yOrder++; zOrder = 0; }

        pos += Vector3.forward * zSpace * zOrder;

        if (moneyList.Count % 2 == 0) pos += Vector3.right * xSpace;
        else zOrder++;

        pos += Vector3.up * ySpace * yOrder;

        money.transform.DOLocalRotate(Vector3.zero, 0.25f);
        money.transform.DOLocalMove(pos, 0.5f);
    }
    public void Interact(Interactor interactor)
    {
        if (interactor.TryGetComponent(out CustomerGetter manager))
        {
            if (!manager.isPlayer) return;

            exit = false;
            StartCoroutine(GiveMoney(manager.transform));
        }
    }

    private IEnumerator GiveMoney(Transform followTransform)
    {
        while (moneyList.Count > 0 || !exit)
        {
            for (int i = 0; i < moneyList.Count; i++)
            {
                Debug.Log("GiveMoney");
                StartCoroutine(moneyList[moneyList.Count - 1].Follow(followTransform));
                moneyList.RemoveAt(moneyList.Count - 1);
                yield return null;
            }
            yield return null;
        }
        zOrder = 0;
        yOrder = 0;

    }

    public void Exit()
    {
        exit = true;
    }
}
