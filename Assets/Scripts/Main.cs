using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    private TextManager m_oTextManager = new TextManager();

    void Start()
    {
        Application.targetFrameRate = 360;

        m_oTextManager.Init();

        //CreateBlinkingText("Test!");

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                CreateBlinkingText("Test!", GetPositionByIndex(new Vector2Int(x + 1, y + 1)));
            }
        }
    }

    private Vector3 GetPositionByIndex(Vector2Int p2Index)
    {
        return new Vector3(p2Index.x * 195 - 489 - 100, p2Index.y * 112 - 281 - 50, 0);
    }

    private void CreateBlinkingText(string sText, Vector3 p3Position)
    {
        GameObject goText = m_oTextManager.CreateText(sText, p3Position);
        StartCoroutine(BlinkText(goText, Helpers.GetRandomFrequency(), Helpers.GetRandomLifeTime(), () =>
        {
            GameObject.Destroy(goText);
        }));
    }

    private IEnumerator BlinkText(GameObject goText, float fFrequency, float fLifeTime, Action fnCalback)
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
}
