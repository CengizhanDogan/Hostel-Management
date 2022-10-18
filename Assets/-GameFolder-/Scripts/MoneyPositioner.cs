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
        money.transform.localScale = Vector3.one;

        pos += Vector3.forward * zSpace * zOrder;
        pos += Vector3.up * ySpace * yOrder;

        if (moneyList.Count % 2 == 0)
        { pos += Vector3.right * xSpace; zOrder++; }

        if (moneyList.Count % 8 == 0) { yOrder++; }

        money.transform.DOLocalRotate(Vector3.zero, 0.25f);
        money.transform.DOLocalMove(pos, 0.5f);

        if (moneyList.Count % 8 == 0) { zOrder = 0; }
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
        while (!exit || moneyList.Count > 0)
        {
            for (int i = 0; i < moneyList.Count; i++)
            {
                StartCoroutine(moneyList[moneyList.Count - 1].Follow(followTransform));
                moneyList.RemoveAt(moneyList.Count - 1);
                yield return null;
            }
            if (moneyList.Count <= 0)
            {
                zOrder = 0;
                yOrder = 0;
            }
            yield return null;
        }

    }

    public void Exit()
    {
        exit = true;
    }
}
