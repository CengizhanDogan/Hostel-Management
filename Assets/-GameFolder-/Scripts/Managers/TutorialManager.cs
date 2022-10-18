using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] private Transform customerTransform;
    [SerializeField] private Transform fridgeTransform;
    public void CustomerTutorial(Transform roomTransform)
    {
        EventManager.OnTutorialEvent.Invoke(customerTransform);
        EventManager.OnTutorialEvent.Invoke(roomTransform);
    }
    public void KitchenTutorial(Transform order)
    {
        EventManager.OnTutorialEvent.Invoke(fridgeTransform);
        EventManager.OnTutorialEvent.Invoke(order);
    }
}
