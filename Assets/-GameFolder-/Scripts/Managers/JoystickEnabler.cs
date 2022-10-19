using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickEnabler : Singleton<JoystickEnabler>
{
    [SerializeField] private GameObject joystickObject;

    public void SetJoystick(bool set)
    {
        joystickObject.SetActive(set);
    }
}
