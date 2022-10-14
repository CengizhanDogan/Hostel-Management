using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickEnabler : Singleton<JoystickEnabler>
{
    [SerializeField] private GameObject joystick;
    
    public void SetJoystick(bool set)
    {
        joystick.SetActive(set);
    }
}
