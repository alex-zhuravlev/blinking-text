using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Core;

public class Main : MonoBehaviour
{
    private const int MAX_X_N = 3;
    private const int MAX_Y_N = 7;

    private TextManager m_oTextManager = new TextManager();
    private Dictionary<Vector2Int, GameObject> m_aTextMatrix = new Dictionary<Vector2Int, GameObject>();

    public string sPoetryText = @"
        Прощай печаль - мать хмурых туч,
        Прощай потертое забвение,
        Прощай ненастье - грусти луч,
        Прощай души моей смятение.

        Спокойно мне и так легко.
        Спокойно плещется мгновение,
        Под звуки правды. Далеко,
        Летит стрела, вне сил сомнения.

        Фанфары счастья не гремят,
        Не выжигает радость тени.
        Спокойно мне. Огни горят,
        Слегка касаясь поколения.

        Видны потоки тишины.
        Их всполохи в сердцах видны.
    ";
    private List<string> m_aWords = new List<string>()
    {
        "прощай", "печаль", "мать", "хмурых", "туч",
        "прощай", "потертое", "забвение",
        "прощай", "ненастье", "грусти луч",
        "прощай", "души моей", "смятение",

        "спокойно", "мне", "и так легко",
        "спокойно", "плещется", "мгновение",
        "под звуки", "правды", "далеко",
        "летит", "стрела", "вне сил", "сомнения",

        "фанфары", "счастья", "не гремят",
        "не выжигает", "радость тени",
        "спокойно", "мне", "огни", "горят",
        "слегка", "касаясь", "поколения",

        "видны", "потоки", "тишины",
        "их всполохи", "в сердцах", "видны"
    };

    public Main()
    {
        /*char[] aText = sPoetryText.Where(Char.IsPunctuation).Distinct().ToArray();
        List<string> aList = sPoetryText.Split().Select(x => x.Trim(aText)).ToList();
        for (int i = 0; i < aList.Count; i++)
        {
            if (aList[i].Length == 0) continue;

            m_aWords.Add(aList[i]);
        }*/

        m_aWords.Shuffle();
    }

    void Start()
    {
        Application.targetFrameRate = 360;

        m_oTextManager.Init();

        StartCoroutine(BlinkLogic());
        // TestCoordsGrid();
    }

    private void TestCoordsGrid()
    {
        for (int x = 0; x < MAX_X_N; x++)
        {
            for (int y = 0; y < MAX_Y_N; y++)
            {
                GameObject goText = m_oTextManager.CreateText("спокойно", GetPositionByIndex(new Vector2Int(x + 1, y + 1)));
            }
        }
    }

    private IEnumerator BlinkLogic()
    {
        for(int i = 0; i < m_aWords.Count; i++)
        {
            Vector2Int p2 = GetRandomPosition();
            Debug.Log(p2);
            if (p2 == Vector2Int.zero)
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

        yield return new WaitForSeconds(5.0f);

        m_oTextManager.CreateText("ⰱⰾⰰⰳⱁⰴⰰⱃⱓ", new Vector3(0, 0, 0));
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
        Vector3 p3Position = new Vector3(40 + p2iIndex.x * 400, 50 + p2iIndex.y * 100, 0);

        // 1
        p3Position.x -= 640;
        p3Position.y -= 400;

        // 2
        p3Position.x -= 250;
        p3Position.y -= 50;

        return p3Position;
    }

    private Vector2Int GetRandomPosition()
    {
        for (int i = 0; i < MAX_X_N * MAX_Y_N; i++)
        {
            int iRandX = enRandom.Get(MAX_X_N) + 1;
            int iRandY = enRandom.Get(MAX_Y_N) + 1;

            Vector2Int p2 = new Vector2Int(iRandX, iRandY);
            if (!m_aTextMatrix.ContainsKey(p2))
                return p2;
        }

        return Vector2Int.zero;
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
