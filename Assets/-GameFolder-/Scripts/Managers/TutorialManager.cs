using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] private Transform customerTransform;
    [SerializeField] private Transform fridgeTransform;
    [SerializeField] private Arrow arrow;
    public void CustomerTutorial(Door door)
    {
        var customer = Reception.Instance.customers[0];
        EventManager.OnTutorialEvent.Invoke(customerTransform);
        EventManager.OnTutorialEvent.Invoke(door.transform);

        var arrow = Instantiate(this.arrow);
        arrow.transform.localScale = Vector3.one;
        arrow.SetFollow(customer.transform, Vector3.up * 5f);
        customer.arrow = arrow.transform;
        arrow = Instantiate(this.arrow);
        arrow.transform.localScale = Vector3.one;
        arrow.SetFollow(door.transform, Vector3.up * 2f);
        door.arrow = arrow.transform;
    }
    public void KitchenTutorial(Order order)
    {
        var fridge = Kitchen.Instance.fridges[0];

        EventManager.OnTutorialEvent.Invoke(fridgeTransform);
        EventManager.OnTutorialEvent.Invoke(order.transform);

        var arrow = Instantiate(this.arrow);
        arrow.transform.localScale = Vector3.one;
        arrow.SetFollow(fridge.transform, Vector3.up * 2f);
        fridge.arrow = arrow.transform;
        arrow = Instantiate(this.arrow);
        arrow.transform.localScale = Vector3.one;
        arrow.SetFollow(order.transform, Vector3.up * 2f);
        order.arrow = arrow.transform;
    }
}
