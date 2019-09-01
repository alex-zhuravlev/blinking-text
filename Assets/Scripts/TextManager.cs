using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Core;

public class CTextManager : ItmSingleton
{
    private static int s_iTextCounter = 1;
    private GameObject m_goCanvas = null;

    public void InitInstance()
    {
        m_goCanvas = GameObject.Find("Canvas");
    }

    public GameObject CreateText(string sText, Vector3 p3Position)
    {
        GameObject goText = new GameObject();
        goText.name = "Text_" + (s_iTextCounter++).ToString();
        goText.transform.parent = m_goCanvas.transform;

        Text oText = goText.AddComponent<Text>();
        oText.font = (Font)Resources.Load("Fonts/seguisym");
        oText.text = sText;
        oText.color = new Color(255.0f, 215.0f, 0.0f);
        oText.fontSize = 72;
        oText.alignment = TextAnchor.MiddleCenter;

        RectTransform oRectTransform = oText.GetComponent<RectTransform>();
        oRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        oRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        oRectTransform.sizeDelta = new Vector2(500, 100);
        oRectTransform.localPosition = p3Position;

        return goText;
    }
}
