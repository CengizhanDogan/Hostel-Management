using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class GameAnalyticsInitialize : MonoBehaviour
{
    void Start()
    {
        GameAnalytics.Initialize();
    }
}
