using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class MenuMain : MonoBehaviour
{
    public void OnButtonMode1Click()
    {
        tmSingleton<CModeManager>.Instance.SwitchMode(tMode.MODE_1);
        tmSingleton<CModeManager>.Instance.HideMenuMain();
    }

    public void OnButtonMode2Click()
    {
        tmSingleton<CModeManager>.Instance.SwitchMode(tMode.MODE_2);
        tmSingleton<CModeManager>.Instance.HideMenuMain();
    }

    public void OnButtonMode3Click()
    {
        tmSingleton<CModeManager>.Instance.SwitchMode(tMode.MODE_3);
        tmSingleton<CModeManager>.Instance.HideMenuMain();
    }
}
