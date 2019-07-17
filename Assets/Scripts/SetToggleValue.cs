using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetToggleValue : MonoBehaviour
{
    [SerializeField]
    string Name = String.Empty;
    [SerializeField]
    bool Default = false;

    private void Awake()
    {
        bool bValue = Convert.ToBoolean(PlayerPrefs.GetInt(Name, Convert.ToInt32(Default)));
        GetComponent<Toggle>().isOn = bValue;
    }

    private void OnDestroy()
    {
        bool bValue = GetComponent<Toggle>().isOn;
        PlayerPrefs.SetInt(Name, Convert.ToInt32(bValue));
    }
}
