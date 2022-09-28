using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.NiceVibrations;

public static class HapticManager
{
    public static UnityEvent OnHaptic = new UnityEvent();

    public static void Haptic(HapticTypes hapticType)
    {
        MMVibrationManager.Haptic(hapticType);
        OnHaptic.Invoke();
    }
}
