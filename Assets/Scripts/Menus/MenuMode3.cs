using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;
using System.IO;
using System;

public class MenuMode3 : MenuMode
{
    public bool ToggleFrequency = false;
    public float SliderSpeed = 0.5f;

    public string Text { get; private set; }

    [SerializeField]
    private GameObject TextEditorPrefab = null;
    private GameObject m_oTextEditorGO = null;
    private InputField m_oInputField = null;
    private string m_sFilePath = String.Empty;

    private void Awake()
    {
        m_sFilePath = Application.persistentDataPath + "/TextMode3.txt";

        if (File.Exists(m_sFilePath))
        {
            Text = File.ReadAllText(m_sFilePath);
        }
    }

    private void OnDisable()
    {
        OnEndEdit();
    }

    public void OnButtonEditTextClick()
    {
        m_oTextEditorGO = GameObject.Instantiate(TextEditorPrefab);
        m_oTextEditorGO.transform.SetParent(GameObject.Find("Canvas").transform);

        RectTransform oRectTransform = m_oTextEditorGO.GetComponent<RectTransform>();
        oRectTransform.sizeDelta = new Vector2(Screen.width * 0.9f, Screen.height * 0.9f);
        oRectTransform.localPosition = Vector3.zero;

        m_oInputField = m_oTextEditorGO.GetComponent<InputField>();
        m_oInputField.text = "";
        m_oInputField.Select();
        m_oInputField.ActivateInputField();

        if (File.Exists(m_sFilePath))
        {
            m_oInputField.text = File.ReadAllText(m_sFilePath);
        }
    }

    public void OnToggleFrequency(Toggle oToggle)
    {
        ToggleFrequency = oToggle.isOn;
    }

    public void OnSliderSpeedChanged(Slider oSlider)
    {
        SliderSpeed = oSlider.value;
    }

    public void OnEndEdit()
    {
        if (m_oTextEditorGO != null)
        {
            File.WriteAllText(m_sFilePath, m_oInputField.text);
            Text = m_oInputField.text;

            GameObject.Destroy(m_oTextEditorGO);
            m_oTextEditorGO = null;
            m_oInputField = null;
        }
    }
}
