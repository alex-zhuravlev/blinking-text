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

    private void Awake()
    {
        float fValue = PlayerPrefs.GetFloat(Name, Default);
        GetComponent<Slider>().value = fValue;
    }

    private void OnDestroy()
    {
        float fValue = GetComponent<Slider>().value;
        PlayerPrefs.SetFloat(Name, fValue);
    }
}
