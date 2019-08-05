using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

public class MenuMode1 : MenuMode
{
    public bool ToggleFrequency = false;
    public bool ToggleWebCam = false;

    public void OnToggleFrequency(Toggle oToggle)
    {
        ToggleFrequency = oToggle.isOn;
    }

    public void OnToggleWebCam(Toggle oToggle)
    {
        ToggleWebCam = oToggle.isOn;
    }
}
