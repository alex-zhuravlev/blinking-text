using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Core;
using UnityEngine.Networking;

public class Main : MonoBehaviour
{
    public TextManager TextManager { get; private set; }

    private GameObject m_oFocusCircle = null;
    private GameObject m_oMenuPanel = null;
    private Mode m_oActiveMode = null;

    public void OnButtonMode1Click()
    {
        if (m_oActiveMode != null)
        {
            m_oActiveMode.Stop();
            m_oActiveMode = null;
        }

        m_oFocusCircle.SetActive(true);

        m_oActiveMode = new Mode1(this);
        m_oActiveMode.Start();

        m_oMenuPanel.SetActive(false);
    }

    public void OnButtonMode2Click()
    {
        if (m_oActiveMode != null)
        {
            m_oActiveMode.Stop();
            m_oActiveMode = null;
        }

        m_oFocusCircle.SetActive(true);

        m_oActiveMode = new Mode2(this);
        m_oActiveMode.Start();

        m_oMenuPanel.SetActive(false);
    }

    public void OnButtonMode3Click()
    {
        if (m_oActiveMode != null)
        {
            m_oActiveMode.Stop();
            m_oActiveMode = null;
        }

        m_oFocusCircle.SetActive(false);

        m_oActiveMode = new Mode3(this);
        m_oActiveMode.Start();

        m_oMenuPanel.SetActive(false);
    }

    public void OnSliderMode3Changed(Slider oSlider)
    {
        if(m_oActiveMode is Mode3)
        {
            Mode3 oMode3 = m_oActiveMode as Mode3;
            oMode3.OnSliderChanged(oSlider.value);
        }
    }

    public void OnToggleMode3Changed(Toggle oToggle)
    {
        if (m_oActiveMode is Mode3)
        {
            Mode3 oMode3 = m_oActiveMode as Mode3;
            oMode3.OnToggleChanged(oToggle.isOn);
        }
    }

    public IEnumerator BlinkText(GameObject goText, float fFrequency, float fLifeTime, Action fnCalback)
    {
        Text oText = goText.GetComponent<Text>();
        string sText = oText.text;
        float fDelay = (1.0f / fFrequency) / 2.0f;

        bool bShow = true;
        while (fLifeTime >= 0)
        {
            oText.text = (bShow) ? sText : String.Empty;
            bShow = !bShow;

            yield return new WaitForSeconds(fDelay);
            fLifeTime -= fDelay;
        }

        fnCalback();
    }

    private void Start()
    {
        Application.targetFrameRate = 360;

        TextManager = new TextManager();
        TextManager.Init();

        m_oFocusCircle = GameObject.Find("FocusCircle");
        PlaceFocusCircle();

        m_oMenuPanel = GameObject.Find("MenuPanel");
        m_oMenuPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_oMenuPanel.SetActive(!m_oMenuPanel.activeInHierarchy);
        }
    }

    private void PlaceFocusCircle()
    {
        RectTransform oRectTransform = m_oFocusCircle.GetComponent<RectTransform>();
        oRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        oRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        oRectTransform.sizeDelta = new Vector2(500, 100);
        oRectTransform.localPosition = new Vector3(-Screen.width * 0.5f, -Screen.height * 0.5f, 0);
    }
}
