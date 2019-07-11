using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Mode3 : Mode
{
    private GameObject m_oActiveText = null;
    private List<string> m_aWords = new List<string>();
    private bool m_bTextDownloaded = false;

    private List<Coroutine> m_aActiveCoroutines = new List<Coroutine>();

    private bool m_bHighFrequency = false;
    private float m_fTextSpeed = 0.5f;

    public Mode3(Main o) : base(o)  { }

    public override void Start()
    {
        Coroutine oCoroutine = m_oMainRef.StartCoroutine(BlinkLogic());
        m_aActiveCoroutines.Add(oCoroutine);
    }

    public override void Stop()
    {
        foreach (Coroutine oCoroutine in m_aActiveCoroutines)
            m_oMainRef.StopCoroutine(oCoroutine);

        GameObject.Destroy(m_oActiveText);
        m_oActiveText = null;
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
        m_oMainRef.StartCoroutine(DownloadPoem());

        yield return new WaitUntil(() => m_bTextDownloaded);

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

    private IEnumerator DownloadPoem()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://116.203.7.150:8000/index.php");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            SplitText(www.downloadHandler.text);
        }

        m_bTextDownloaded = true;
    }

    private void SplitText(string sText)
    {
        m_aWords.Clear();

        char[] aText = sText.Where(Char.IsPunctuation).Distinct().ToArray();
        List<string> aList = sText.Split().Select(x => x.Trim(aText)).ToList();
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
        m_oActiveText = m_oMainRef.TextManager.CreateText(sText, new Vector3(0, 0, 0));

        Coroutine oCoroutine = m_oMainRef.StartCoroutine(m_oMainRef.BlinkText(m_oActiveText, Helpers.GetFrequency(Convert.ToInt32(m_bHighFrequency)), Math.Max(1.0f - m_fTextSpeed, 0.05f) * 3.0f, () =>
        {
            GameObject.Destroy(m_oActiveText);
            m_oActiveText = null;
        }));
        m_aActiveCoroutines.Add(oCoroutine);
    }
}
