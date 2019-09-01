using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mode1 : Mode
{
    [SerializeField]
    private GameObject FocusCirclePrefab = null;
    private GameObject m_oFocusCircleGO = null;

    private Dictionary<Vector2Int, GameObject> m_aTextMatrix = new Dictionary<Vector2Int, GameObject>();
    private bool m_bHighFrequency = false;

    private MenuMode1 m_oMenuMode1 = null;

    protected override void Awake()
    {
        base.Awake();

        m_oMenuMode1 = m_oMenuGO.GetComponent<MenuMode1>();

        m_oFocusCircleGO = GameObject.Instantiate<GameObject>(FocusCirclePrefab);
        m_oFocusCircleGO.transform.SetParent(m_oCanvasGO.transform);
        
        RectTransform oRectTransform = m_oFocusCircleGO.GetComponent<RectTransform>();
        oRectTransform.localPosition = new Vector3(-Screen.width * 0.5f, -Screen.height * 0.5f, 0);

        m_bHighFrequency = Convert.ToBoolean(PlayerPrefs.GetInt("Mode1_bHighFrequency", 0));
    }

    protected override void Start()
    {
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
        m_bHighFrequency = m_oMenuMode1.ToggleFrequency;
    }

    private void OnDestroy()
    {
        foreach (KeyValuePair<Vector2Int, GameObject> record in m_aTextMatrix)
        {
            GameObject.Destroy(record.Value);
        }
        m_aTextMatrix.Clear();

        if(m_oFocusCircleGO != null) GameObject.Destroy(m_oFocusCircleGO);
    }

    private IEnumerator BlinkLogic()
    {
        int iIndex = Convert.ToInt32(m_bHighFrequency);

        CreateBlinkingText("Я", new Vector2Int(0, 0), Helpers.Frequencies[iIndex]);
        CreateBlinkingText("Есть", new Vector2Int(1, 0), Helpers.Frequencies[1 - iIndex]);

        yield return null;
    }

    private void CreateBlinkingText(string sText, Vector2Int p2iPosition, float fFrequency)
    {
        GameObject goText = tmSingleton<CTextManager>.Instance.CreateText(sText, GetPositionByIndex(p2iPosition));
        m_aTextMatrix.Add(p2iPosition, goText);

        StartCoroutine(BlinkText(goText, fFrequency, float.MaxValue, () =>
        {
            m_aTextMatrix.Remove(p2iPosition);
            GameObject.Destroy(goText);
        }));
    }

    private Vector3 GetPositionByIndex(Vector2Int p2iIndex)
    {
        float fW = 250.0f;

        if (p2iIndex == new Vector2Int(0, 0))
        {
            return new Vector3(-fW, 0, 0);
        }
        if (p2iIndex == new Vector2Int(1, 0))
        {
            return new Vector3(fW, 0, 0);
        }

        return new Vector3Int();
    }
}
