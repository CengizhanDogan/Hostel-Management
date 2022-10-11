using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrashCan : MonoBehaviour, IInteractable
{
    public void Interact(Interactor interactor)
    {
        var delivery = interactor.GetComponent<FoodDelivery>();

        Food food = delivery.GetFood();
        if (!food) return;

        food.SetFollowTransform(null);
        delivery.SetFood(null);
        PlayerAnimatorController anim = null;
        if (interactor.TryGetComponent(out CustomerGetter cg))
            anim = interactor.GetComponentInChildren<PlayerAnimatorController>();

        food.transform.DOJump(transform.position, 1, 1, 0.5f).OnUpdate(() =>
        {
            food.transform.localScale -= Vector3.one * 0.025f;
        })
            .OnComplete(() =>
        {
            if (anim) anim.SetTrayAnimation(false);
            Destroy(food.gameObject);
        });
    }
}
