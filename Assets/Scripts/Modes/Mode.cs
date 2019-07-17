using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Mode : MonoBehaviour
{
    [SerializeField]
    protected GameObject MenuPrefab = null;
    protected GameObject m_oCanvasGO = null;
    protected GameObject m_oMenuGO = null;

    private GraphicRaycaster m_oGraphicRaycaster = null;

    protected virtual void Awake()
    {
        m_oCanvasGO = GameObject.Find("Canvas");

        m_oMenuGO = GameObject.Instantiate<GameObject>(MenuPrefab);

        m_oGraphicRaycaster = m_oCanvasGO.GetComponent<GraphicRaycaster>();

        m_oMenuGO.transform.SetParent(m_oCanvasGO.transform, false);
        m_oMenuGO.SetActive(false);
    }

    protected virtual void Start() { }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PointerEventData oPointerData = new PointerEventData(EventSystem.current);
            oPointerData.position = Input.mousePosition;

            List<RaycastResult> aRaycastResults = new List<RaycastResult>();
            m_oGraphicRaycaster.Raycast(oPointerData, aRaycastResults);

            //foreach (RaycastResult result in results)
            //{
            //    Debug.Log("Hit " + result.gameObject.name);
            //}

            if (aRaycastResults.Count == 0) // Clicked outside of any UI elements
            {
                if(m_oMenuGO.activeInHierarchy)
                {
                    m_oMenuGO.SetActive(false);
                    OnMenuClosed();
                    Resume();
                }
                else
                {
                    m_oMenuGO.SetActive(true);
                    Pause();
                }
            }
        }
    }

    protected virtual void Pause() { }

    protected virtual void Resume() { }

    protected virtual void OnMenuClosed() { }

    public IEnumerator BlinkText(GameObject goText, float fFrequency, float fLifeTime, Action fnCalback)
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
