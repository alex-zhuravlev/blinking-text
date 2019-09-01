using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Mode3 : Mode
{
    private GameObject m_oActiveText = null;
    private List<string> m_aWords = new List<string>();

    private bool m_bHighFrequency = false;
    private float m_fTextSpeed = 0.5f;

    private MenuMode3 m_oMenuMode3 = null;

    protected override void Awake()
    {
        base.Awake();

        m_oMenuMode3 = m_oMenuGO.GetComponent<MenuMode3>();

        m_bHighFrequency = Convert.ToBoolean(PlayerPrefs.GetInt("Mode3_bHighFrequency", 0));
        m_fTextSpeed = PlayerPrefs.GetFloat("Mode3_fTextSpeed", 0.5f);
    }

    protected override void Start()
    {
        SplitText(m_oMenuMode3.Text);
        StartCoroutine(BlinkLogic());
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Pause()
    {
        StopAllCoroutines();

        if (m_oActiveText != null)
        {
            GameObject.Destroy(m_oActiveText);
            m_oActiveText = null;
        }
    }

    protected override void Resume()
    {
        StartCoroutine(BlinkLogic());
    }

    protected override void OnMenuClosed()
    {
        SplitText(m_oMenuMode3.Text);
        m_fTextSpeed = m_oMenuMode3.SliderSpeed;
    }

    private void OnDestroy()
    {
        if (m_oActiveText != null)
        {
            GameObject.Destroy(m_oActiveText);
            m_oActiveText = null;
        }
    }

    public void OnToggleChanged(bool bValue)
    {
        m_bHighFrequency = bValue;
    }

    public void OnSliderChanged(float fValue)
    {
        m_fTextSpeed = fValue;
    }

    private IEnumerator BlinkLogic()
    {
        while (true)
        {
            for (int i = 0; i < m_aWords.Count; i++)
            {
                CreateBlinkingText(m_aWords[i]);

                yield return new WaitUntil(() => m_oActiveText == null);
            }

            yield return new WaitForSeconds(3.0f);
        }
    }

    private void SplitText(string sText)
    {
        m_aWords.Clear();

        char[] aText = sText.Where(Char.IsPunctuation).Distinct().ToArray();
        List<string> aList = sText.ToLower().Split().Select(x => x.Trim(aText)).ToList();
        for (int i = 0; i < aList.Count; i++)
        {
            if (aList[i].Length == 0) continue;

            if (aList[i].Length <= 3 && i + 1 < aList.Count && aList[i + 1].Length <= 8)
            {
                m_aWords.Add(aList[i] + " " + aList[i + 1]);
                ++i;
                continue;
            }
            if (aList[i].Length <= 8 && i + 1 < aList.Count && aList[i + 1].Length <= 3)
            {
                m_aWords.Add(aList[i] + " " + aList[i + 1]);
                ++i;
                continue;
            }

            m_aWords.Add(aList[i]);
        }
    }

    private void CreateBlinkingText(string sText)
    {
        m_oActiveText = tmSingleton<CTextManager>.Instance.CreateText(sText, new Vector3(0, 0, 0));

        const float fValueMin = 10.0f;
        const float fValueMiddle = 60.0f;
        const float fValueMax = 500.0f;

        int iWordsPerMinute = (int)fValueMiddle;
        if (m_fTextSpeed < 0.5f)
        {
            iWordsPerMinute = (int)(fValueMin + (fValueMiddle - fValueMin) * (m_fTextSpeed / 0.5f));
        }
        else if (m_fTextSpeed > 0.5f)
        {
            iWordsPerMinute = (int)(fValueMiddle + (fValueMax - fValueMiddle) * (m_fTextSpeed / 0.5f - 1.0f));
        }

        float fTextDisplayTime = (1.0f / (float)iWordsPerMinute) * 60.0f;

        StartCoroutine(BlinkText(m_oActiveText, Helpers.GetFrequency(Convert.ToInt32(m_bHighFrequency)), fTextDisplayTime, () =>
        {
            GameObject.Destroy(m_oActiveText);
            m_oActiveText = null;
        }));
    }
}
