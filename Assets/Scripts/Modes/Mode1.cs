using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Mode1 : Mode
{
    private Dictionary<Vector2Int, GameObject> m_aTextMatrix = new Dictionary<Vector2Int, GameObject>();
    private List<Coroutine> m_aActiveCoroutines = new List<Coroutine>();

    private static int m_iRandomFreqIndex = enRandom.Get(2);

    public Mode1(Main o) : base(o)  { }

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
        m_iRandomFreqIndex = 1 - m_iRandomFreqIndex;

        CreateBlinkingText("Я", new Vector2Int(0, 0), Helpers.Frequencies[m_iRandomFreqIndex]);
        CreateBlinkingText("Есть", new Vector2Int(1, 0), Helpers.Frequencies[1 - m_iRandomFreqIndex]);

        yield return null;
    }

    private void CreateBlinkingText(string sText, Vector2Int p2iPosition, float fFrequency)
    {
        GameObject goText = m_oMainRef.TextManager.CreateText(sText, GetPositionByIndex(p2iPosition));
        m_aTextMatrix.Add(p2iPosition, goText);

        Coroutine oCoroutine = m_oMainRef.StartCoroutine(m_oMainRef.BlinkText(goText, fFrequency, float.MaxValue, () =>
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
