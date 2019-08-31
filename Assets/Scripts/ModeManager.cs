using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public enum tMode
{
    MODE_1 = 0,
    MODE_2,
    MODE_3,

    MODE_NONE
}

public class CModeManager : ItmSingleton
{
    private Dictionary<tMode, string> m_aModeConf = new Dictionary<tMode, string>()
    {
        { tMode.MODE_1, "Prefabs/Mode1"},
        { tMode.MODE_2, "Prefabs/Mode2"},
        { tMode.MODE_3, "Prefabs/Mode3"}
    };

    private GameObject m_oMenuMainGO = null;
    private GameObject m_oActiveModeGO = null;

    public void InitInstance()
    {
        m_oMenuMainGO = GameObject.Find("MenuMain");
    }

    public void SwitchMode(tMode eMode)
    {
        if (m_oActiveModeGO != null)
        {
            GameObject.Destroy(m_oActiveModeGO);
            m_oActiveModeGO = null;
        }

        if (eMode != tMode.MODE_NONE)
        {
            GameObject oModePrefab = Resources.Load<GameObject>(m_aModeConf[eMode]);
            m_oActiveModeGO = GameObject.Instantiate(oModePrefab);
        }
    }

    public void ShowMenuMain()
    {
        //m_oMenuMainGO.SetActive(true);
    }

    public void HideMenuMain()
    {
        m_oMenuMainGO.SetActive(false);
    }
}
