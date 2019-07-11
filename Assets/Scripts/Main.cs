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
    private const int MAX_X_N = 2;
    private const int MAX_Y_N = 2;

    private TextManager m_oTextManager = new TextManager();
    private Dictionary<Vector2Int, GameObject> m_aTextMatrix = new Dictionary<Vector2Int, GameObject>();

    private List<string> m_aWords = new List<string>();

    private bool m_bTextDownloaded = false;

    void Start()
    {
        Application.targetFrameRate = 360;

        m_oTextManager.Init();

        StartCoroutine(DownloadPoem());

        StartCoroutine(BlinkLogic());
        // TestCoordsGrid();
    }

    IEnumerator DownloadPoem()
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

        m_aWords.Shuffle();
    }

    private void TestCoordsGrid()
    {
        PlaceFocusCircle();

        for (int x = 0; x < MAX_X_N; x++)
        {
            for (int y = 0; y < MAX_Y_N; y++)
            {
                m_oTextManager.CreateText("спокойно", GetPositionByIndex(new Vector2Int(x, y)));
            }
        }
    }

    private IEnumerator BlinkLogic()
    {
        yield return new WaitUntil(() => m_bTextDownloaded);

        PlaceFocusCircle();

        while (true)
        {
            for (int i = 0; i < m_aWords.Count; i++)
            {
                Vector2Int p2 = GetRandomPosition();
                Debug.Log(p2);
                if (p2 == new Vector2Int(-1, -1))
                {
                    // No more free slots. Wait
                    i--;
                }
                else
                {
                    CreateBlinkingText(m_aWords[i], p2);
                }

                yield return new WaitForSeconds(Helpers.GetRandomLifeTime());
            }

            yield return new WaitForSeconds(3.0f);
        }
    }

    private void PlaceFocusCircle()
    {
        GameObject oFocusCircle = GameObject.Find("focus-circle");
        RectTransform oRectTransform = oFocusCircle.GetComponent<RectTransform>();
        oRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        oRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        oRectTransform.sizeDelta = new Vector2(500, 100);
        oRectTransform.localPosition = new Vector3(-Screen.width * 0.5f, -Screen.height * 0.5f, 0);
    }

    private void CreateBlinkingText(string sText, Vector2Int p2iPosition)
    {
        GameObject goText = m_oTextManager.CreateText(sText, GetPositionByIndex(p2iPosition));
        m_aTextMatrix.Add(p2iPosition, goText);

        StartCoroutine(BlinkText(goText, Helpers.GetRandomFrequency(), Helpers.GetRandomLifeTime(), () =>
        {
            m_aTextMatrix.Remove(p2iPosition);
            GameObject.Destroy(goText);
        }));
    }

    private Vector3 GetPositionByIndex(Vector2Int p2iIndex)
    {
        if(p2iIndex == new Vector2Int(0, 0))
        {
            return new Vector3(-220, -100, 0);
        }
        if (p2iIndex == new Vector2Int(0, 1))
        {
            return new Vector3(-220, 100, 0);
        }
        if (p2iIndex == new Vector2Int(1, 0))
        {
            return new Vector3(220, -100, 0);
        }
        if (p2iIndex == new Vector2Int(1, 1))
        {
            return new Vector3(220, 100, 0);
        }

        return new Vector3Int();
    }

    private Vector2Int GetRandomPosition()
    {
        for (int i = 0; i < 10 * MAX_X_N * MAX_Y_N; i++)
        {
            int iRandX = enRandom.Get(MAX_X_N);
            int iRandY = enRandom.Get(MAX_Y_N);

            Vector2Int p2 = new Vector2Int(iRandX, iRandY);
            if (!m_aTextMatrix.ContainsKey(p2))
                return p2;
        }

        return new Vector2Int(-1, -1);
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
