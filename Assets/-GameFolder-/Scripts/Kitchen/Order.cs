using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Order : MonoBehaviour, IInteractable
{
    [SerializeField] private List<GameObject> orderBubbles = new List<GameObject>();
    private Vector3 scale;
    private Collider coll;
    public Room room;

    private void Start()
    {
        coll = GetComponent<Collider>();
        scale = transform.parent.localScale;
        transform.parent.localScale = Vector3.zero;
    }
    public void SetBubbles(bool set)
    {
        var scale = Vector3.zero;

        if (set)
        {
            foreach (var item in orderBubbles)
            {
                item.SetActive(false);
            }

            orderBubbles[PlayerPrefs.GetInt(PlayerPrefKeys.KitchenLevel)].SetActive(true);

            scale = this.scale;
        }

        coll.enabled = set;

        transform.parent.DOScale(scale, 0.5f);
    }

    public void Interact(Interactor interactor)
    {
        if (interactor.TryGetComponent(out FoodDelivery delivery))
        {
            DOTween.Complete(this);
            Food food = delivery.GetFood();
            if (!food) return;

            PlayerAnimatorController anim = null;
            if (interactor.TryGetComponent(out CustomerGetter cg))
                anim = interactor.GetComponentInChildren<PlayerAnimatorController>();

            SetBubbles(false);

            var spawnPos = food.transform.position; spawnPos.y += 1.5f;
            var particle = PoolingSystem.Instance.InstantiateAPS("FoodSpark", spawnPos);

            food.transform.DOScale(0, 0.5f).OnComplete(() =>
            {
                particle.transform.DOScale(2f, 5f).OnComplete(() => 
                PoolingSystem.Instance.DestroyAPS(particle));

                if (anim) anim.SetTrayAnimation(false);
                delivery.SetFood(null);
                Destroy(food.gameObject);
            });

            room.GetCustomer().GetComponent<FoodOrder>().OrderDone();
        }
    }
}
