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
    [SerializeField] private Transform tutorialTransform;

    [HideInInspector] public Transform arrow;
    private void Start()
    {
        if (PlayerPrefs.GetInt(PlayerPrefKeys.KitchenLevel) > 2)
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.KitchenLevel, 2);
        }
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

            var bubble = orderBubbles[PlayerPrefs.GetInt(PlayerPrefKeys.KitchenLevel)];
            bubble.SetActive(true);

            scale = this.scale;

            if (PlayerPrefs.GetInt(PlayerPrefKeys.KitchenTutorial) == 0)
            {
                PlayerPrefs.SetInt(PlayerPrefKeys.KitchenTutorial, 1);
                TutorialManager.Instance.KitchenTutorial(this);
            }
        }

        coll.enabled = set;

        transform.parent.DOScale(scale, 0.5f);
        if (arrow) arrow.DOScale(scale, 0.5f);
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
            if (arrow) Destroy(arrow.gameObject);

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
