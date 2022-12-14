using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] private RoomBehaviour room;
    public void SitEvent()
    {
        if (!transform.parent.GetComponent<CustomerBehaviour>().room) return;
        room = transform.parent.GetComponent<RoomBehaviour>();
        room.sleep.sat = true;
    }
}
