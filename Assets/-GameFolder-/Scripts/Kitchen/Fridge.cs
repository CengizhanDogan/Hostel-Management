using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Fridge : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform fridge;
    [SerializeField] private List<Transform> fridgeDoor = new List<Transform>();
    [SerializeField] private Food food;
    private Collider coll;

    [HideInInspector] public Transform arrow;

    private void Start()
    {
        coll = GetComponent<Collider>();
    }
    public void Interact(Interactor interactor)
    {
        int carryInt;
        PlayerAnimatorController anim = null;
        if (interactor.TryGetComponent(out CustomerGetter cg))
        {
            anim = interactor.GetComponentInChildren<PlayerAnimatorController>();
            carryInt = PlayerPrefs.GetInt(PlayerPrefKeys.PlayerCarryCount) + 1;
        }
        else
        {
            carryInt = PlayerPrefs.GetInt(PlayerPrefKeys.CookCarryCount) + 1;
        }


        for (int i = 0; i < carryInt; i++)
        {
            var delivery = interactor.GetComponent<FoodDelivery>();
            if (delivery.FoodCount >= carryInt) return;

            var spawnPos = fridge.position; spawnPos.y += 1f;
            var food = Instantiate(this.food, spawnPos, this.food.transform.rotation);
            delivery.SetFood(food);

            if (arrow) Destroy(arrow.gameObject);

            fridgeDoor[0].DOLocalRotate(Vector3.up * 120, 0.5f)
                .OnComplete(() =>
                {
                    StartCoroutine(SendFood(food, delivery, interactor));
                    if (anim) anim.SetTrayAnimation(true);
                });
            if (fridgeDoor.Count > 1)
            {
                fridgeDoor[1].DOLocalRotate(Vector3.up * -120, 0.5f)
                    .OnComplete(() =>
                    {
                        StartCoroutine(SendFood(food, delivery, interactor));
                        if (anim) anim.SetTrayAnimation(true);
                    });
            }
        }
    }

    private IEnumerator SendFood(Food food, FoodDelivery delivery, Interactor interactor)
    {
        Vector3 pos = delivery.CarryTransform.position;
        while (Vector3.Distance(food.transform.position, pos) > 0.01f)
        {
            pos = delivery.CarryTransform.position;
            pos.y += delivery.GetFoodOrder(food) * 0.15f;

            food.transform.position =
                Vector3.Lerp(food.transform.position,
                pos, 20 * Time.deltaTime);
            yield return null;
        }

        food.SetFollowTransform(delivery.CarryTransform);

        foreach (var item in fridgeDoor)
        {
            item.DOLocalRotate(Vector3.zero, 0.5f);
        }
    }

    public void SetColl(bool set)
    {
        coll.enabled = set;
    }
}
