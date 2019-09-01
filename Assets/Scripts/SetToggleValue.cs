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
    [SerializeField]
    GameObject LabelGO = null;
    [SerializeField]
    string TextOn = String.Empty;
    [SerializeField]
    string TextOff = String.Empty;

    private void Awake()
    {
        bool bValue = Convert.ToBoolean(PlayerPrefs.GetInt(Name, Convert.ToInt32(Default)));
        Toggle oToggle = GetComponent<Toggle>();
        oToggle.isOn = bValue;
        oToggle.onValueChanged.AddListener(delegate
        {
            OnValueChanged(oToggle);
        });

        UpdateLabelText();
    }

    private void OnDestroy()
    {
        bool bValue = GetComponent<Toggle>().isOn;
        PlayerPrefs.SetInt(Name, Convert.ToInt32(bValue));
    }

    public void OnValueChanged(Toggle oToggle)
    {
        UpdateLabelText();
    }

    private bool CanUpdateText()
    {
        return (LabelGO != null && TextOn != String.Empty && TextOff != String.Empty);
    }

    private void UpdateLabelText()
    {
        if (CanUpdateText())
        {
            LabelGO.GetComponent<Text>().text = GetComponent<Toggle>().isOn ? TextOn : TextOff;
        }
    }
}
