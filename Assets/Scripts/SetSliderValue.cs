using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSliderValue : MonoBehaviour
{
    [SerializeField]
    string Name = String.Empty;
    [SerializeField]
    float Default = 0.5f;
    [SerializeField]
    GameObject LabelGO = null;
    [SerializeField]
    string TextTemplate = String.Empty;
    [SerializeField]
    float ValueMin = 0.0f;
    [SerializeField]
    float ValueMiddle = 0.0f;
    [SerializeField]
    float ValueMax = 0.0f;

    private void Awake()
    {
        float fValue = PlayerPrefs.GetFloat(Name, Default);
        Slider oSlider = GetComponent<Slider>();
        oSlider.value = fValue;
        oSlider.onValueChanged.AddListener(delegate
        {
            OnValueChanged(oSlider);
        });

        UpdateLabelText();
    }

    private void OnDestroy()
    {
        float fValue = GetComponent<Slider>().value;
        PlayerPrefs.SetFloat(Name, fValue);
    }

    public void OnValueChanged(Slider oSlider)
    {
        UpdateLabelText();
    }

    private bool CanUpdateText()
    {
        return (LabelGO != null && TextTemplate != String.Empty && ValueMiddle > 0.0f && ValueMax >= 0.0f);
    }

    private void UpdateLabelText()
    {
        if (CanUpdateText())
        {
            float fValue = GetComponent<Slider>().value;

            int iLabelValue = (int)ValueMiddle;
            if (fValue < 0.5f)
            {
                iLabelValue = (int)(ValueMin + (ValueMiddle - ValueMin) * (fValue / 0.5f));
            }
            else if(fValue > 0.5f)
            {
                iLabelValue = (int)(ValueMiddle + (ValueMax - ValueMiddle) * (fValue / 0.5f - 1.0f));
            }

            LabelGO.GetComponent<Text>().text = String.Format(TextTemplate, iLabelValue);
        }
    }
}
