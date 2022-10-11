using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Fridge : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform fridge;
    [SerializeField] private Transform fridgeDoor;
    [SerializeField] private Food food;
    private Collider coll;

    private void Start()
    {
        coll = GetComponent<Collider>();
    }
    public void Interact(Interactor interactor)
    {
        var delivery = interactor.GetComponent<FoodDelivery>();
        if (delivery.GetFood()) return;

        

        var spawnPos = fridge.position; spawnPos.y += 1f;
        var food = Instantiate(this.food, spawnPos, this.food.transform.rotation);
        delivery.SetFood(food);

        fridgeDoor.DOLocalRotate(Vector3.up * 120, 0.5f)
            .OnComplete(() =>
            {
                StartCoroutine(SendFood(food, delivery, interactor));
            });
    }

    private IEnumerator SendFood(Food food, FoodDelivery delivery, Interactor interactor)
    {
        while (Vector3.Distance(food.transform.position, delivery.CarryTransform.position) > 0.1f)
        {
            food.transform.position =
                Vector3.Lerp(food.transform.position,
                delivery.CarryTransform.position, 10 * Time.deltaTime);
            yield return null;
        }

        PlayerAnimatorController anim = null;
        if (interactor.TryGetComponent(out CustomerGetter cg))
            anim = interactor.GetComponentInChildren<PlayerAnimatorController>();

        food.SetFollowTransform(delivery.CarryTransform);

        if (anim) anim.SetTrayAnimation(true);

        fridgeDoor.DOLocalRotate(Vector3.zero, 0.5f);
    }

    public void SetColl(bool set)
    {
        coll.enabled = set;
    }
}
