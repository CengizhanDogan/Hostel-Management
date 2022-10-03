using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour, IInteractable, IExitable
{
    [SerializeField] private Room room;
    [SerializeField] private Rigidbody doorRb;
    [SerializeField] private Transform longWalls;

    private bool exited;
    private Collider coll;

    private void Start()
    {
        coll = GetComponent<Collider>();
        PlayerPrefs.SetInt(PlayerPrefKeys.Coin, 1000);
    }

    public void Interact(ManagerBehaviour manager)
    {
        exited = false;

        if (room.available)
        {
            if (manager.GetCustomer() == null || room.GetCustomer() != null) return;

            var customer = manager.GetCustomer();
            customer.SetToRoom(room);
            room.SetCustomer(customer);
        }
        else
        {
            StartCoroutine(SpendMoneyToRoom());
        }
    }

    private IEnumerator SpendMoneyToRoom()
    {
        while (!exited)
        {
            if (PlayerPrefs.GetInt(PlayerPrefKeys.Coin) > 0 && room.roomValue > 0)
            {
                PlayerPrefs.SetInt(PlayerPrefKeys.Coin, PlayerPrefs.GetInt(PlayerPrefKeys.Coin) - 1);
                room.roomValue -= 1;
                foreach (var item in FindObjectOfType<LevelPanel>().inGameCoinTexts)
                {
                    item.text = PlayerPrefs.GetInt(PlayerPrefKeys.Coin).ToString();
                }
            }
            if (room.roomValue <= 0)
            {
                coll.enabled = false;
               
                room.transform.DOMoveY(0, 1f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    coll.enabled = true;
                    doorRb.isKinematic = false;
                    room.available = true;
                });
                longWalls.transform.DOMoveY(-3, 1f).SetEase(Ease.OutBack);
            }
            yield return new WaitForSeconds(0.15f);
        }
    }
    public void Exit()
    {
        exited = true;
    }
}
