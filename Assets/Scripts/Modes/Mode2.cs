using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Mode2 : Mode
{
    private const int MAX_X_N = 2;
    private const int MAX_Y_N = 2;

    private Dictionary<Vector2Int, GameObject> m_aTextMatrix = new Dictionary<Vector2Int, GameObject>();
    private List<string> m_aWords = new List<string>();
    private bool m_bTextDownloaded = false;

    private List<Coroutine> m_aActiveCoroutines = new List<Coroutine>();

    public Mode2(Main o) : base(o)  { }

    public override void Start()
    {
        Coroutine oCoroutine = m_oMainRef.StartCoroutine(BlinkLogic());
        m_aActiveCoroutines.Add(oCoroutine);
    }

    public override void Stop()
    {
        foreach (Coroutine oCoroutine in m_aActiveCoroutines)
            m_oMainRef.StopCoroutine(oCoroutine);

        RemoveAllTexts();
    }

    private IEnumerator BlinkLogic()
    {
        m_oMainRef.StartCoroutine(DownloadPoem());

        yield return new WaitUntil(() => m_bTextDownloaded);

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

        m_aWords.Shuffle();
    }

    private void TestCoordsGrid()
    {
        for (int x = 0; x < MAX_X_N; x++)
        {
            for (int y = 0; y < MAX_Y_N; y++)
            {
                m_oMainRef.TextManager.CreateText("спокойно", GetPositionByIndex(new Vector2Int(x, y)));
            }
        }
    }

    private void CreateBlinkingText(string sText, Vector2Int p2iPosition)
    {
        GameObject goText = m_oMainRef.TextManager.CreateText(sText, GetPositionByIndex(p2iPosition));
        m_aTextMatrix.Add(p2iPosition, goText);

        Coroutine oCoroutine = m_oMainRef.StartCoroutine(m_oMainRef.BlinkText(goText, Helpers.GetRandomFrequency(), Helpers.GetRandomLifeTime(), () =>
        {
            m_aTextMatrix.Remove(p2iPosition);
            GameObject.Destroy(goText);
        }));
        m_aActiveCoroutines.Add(oCoroutine);
    }

    private void RemoveAllTexts()
    {
        foreach (KeyValuePair<Vector2Int, GameObject> record in m_aTextMatrix)
        {
            GameObject.Destroy(record.Value);
        }
        m_aTextMatrix.Clear();
    }

    private Vector3 GetPositionByIndex(Vector2Int p2iIndex)
    {
        float fW = 250.0f;
        float fH = 120.0f;

        if (p2iIndex == new Vector2Int(0, 0))
        {
            return new Vector3(-fW, -fH, 0);
        }
        if (p2iIndex == new Vector2Int(0, 1))
        {
            return new Vector3(-fW, fH, 0);
        }
        if (p2iIndex == new Vector2Int(1, 0))
        {
            return new Vector3(fW, -fH, 0);
        }
        if (p2iIndex == new Vector2Int(1, 1))
        {
            return new Vector3(fW, fH, 0);
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
}
