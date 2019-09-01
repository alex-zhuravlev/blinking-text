using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Mode2 : Mode
{
    private const int MAX_X_N = 2;
    private const int MAX_Y_N = 2;

    [SerializeField]
    protected GameObject FocusCirclePrefab = null;
    private GameObject m_oFocusCircleGO = null;

    private Dictionary<Vector2Int, GameObject> m_aTextMatrix = new Dictionary<Vector2Int, GameObject>();
    private List<string> m_aWords = new List<string>();

    private MenuMode2 m_oMenuMode2 = null;

    protected override void Awake()
    {
        base.Awake();

        m_oMenuMode2 = m_oMenuGO.GetComponent<MenuMode2>();

        m_oFocusCircleGO = GameObject.Instantiate<GameObject>(FocusCirclePrefab);
        m_oFocusCircleGO.transform.SetParent(m_oCanvasGO.transform);

        RectTransform oRectTransform = m_oFocusCircleGO.GetComponent<RectTransform>();
        oRectTransform.localPosition = new Vector3(-Screen.width * 0.5f, -Screen.height * 0.5f, 0);
    }

    protected override void Start()
    {
        SplitText(m_oMenuMode2.Text);
        StartCoroutine(BlinkLogic());
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Pause()
    {
        StopAllCoroutines();

        foreach (KeyValuePair<Vector2Int, GameObject> record in m_aTextMatrix)
        {
            GameObject.Destroy(record.Value);
        }
        m_aTextMatrix.Clear();

        m_oFocusCircleGO.SetActive(false);
    }

    protected override void Resume()
    {
        StartCoroutine(BlinkLogic());

        m_oFocusCircleGO.SetActive(true);
    }

    protected override void OnMenuClosed()
    {
        SplitText(m_oMenuMode2.Text);
    }

    private void OnDestroy()
    {
        foreach (KeyValuePair<Vector2Int, GameObject> record in m_aTextMatrix)
        {
            GameObject.Destroy(record.Value);
        }
        m_aTextMatrix.Clear();

        if (m_oFocusCircleGO != null) GameObject.Destroy(m_oFocusCircleGO);
    }

    private IEnumerator BlinkLogic()
    {
        while (true)
        {
            for (int i = 0; i < m_aWords.Count; i++)
            {
                Vector2Int p2 = GetRandomPosition();
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

        m_aWords.Shuffle();
    }


    private void CreateBlinkingText(string sText, Vector2Int p2iPosition)
    {
        GameObject goText = tmSingleton<CTextManager>.Instance.CreateText(sText, GetPositionByIndex(p2iPosition));
        m_aTextMatrix.Add(p2iPosition, goText);

        StartCoroutine(BlinkText(goText, Helpers.GetRandomFrequency(), Helpers.GetRandomLifeTime(), () =>
        {
            m_aTextMatrix.Remove(p2iPosition);
            GameObject.Destroy(goText);
        }));
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
