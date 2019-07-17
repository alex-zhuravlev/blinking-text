using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

public class MenuMode : MonoBehaviour
{
    public void OnButtonMainMenuClick()
    {
        GameObject.Destroy(gameObject);

        tmSingleton<CModeManager>.Instance.SwitchMode(tMode.MODE_NONE);
        tmSingleton<CModeManager>.Instance.ShowMenuMain();
    }
}
