using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Core;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class Main : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 360;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        tmSingleton<CModeManager>.Instance.ShowMenuMain();
    }

    private void Start()
    {

    }

    private void Update()
    {
        
    }
}
