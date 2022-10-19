using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickEnabler : Singleton<JoystickEnabler>
{
    [SerializeField] private GameObject joystickObject;
    [SerializeField] private Transform handle;

    public void SetJoystick(bool set)
    {
        handle.localPosition = Vector3.zero;
        joystickObject.SetActive(set);
    }
}
