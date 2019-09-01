using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Core;
using UnityEngine.UI;

public class WebCam : ItmSingleton
{
    public GameObject WebCamGO { get; private set; }

    private WebCamTexture m_oWebCamTexture = null;

    public WebCam()
    {
        // Request permissions
        Application.RequestUserAuthorization(UserAuthorization.WebCam);

        // Create game objects
        GameObject oWebCamPrefab = Resources.Load<GameObject>("Prefabs/WebCam");
        Debug.Assert(oWebCamPrefab != null);

        GameObject oCanvasGO = GameObject.Find("Canvas");
        Debug.Assert(oCanvasGO != null);

        WebCamGO = GameObject.Instantiate<GameObject>(oWebCamPrefab);
        WebCamGO.transform.SetParent(oCanvasGO.transform);

        // Set fullscreen & rotate
        RectTransform oWebCamRT = WebCamGO.GetComponent<RectTransform>();
        oWebCamRT.rotation = Quaternion.AngleAxis(90, Vector3.forward);
        oWebCamRT.sizeDelta = new Vector2(Screen.height, Screen.width);
        oWebCamRT.localPosition = Vector3.zero;

        // Get font-facing device
        string sDeviceName = String.Empty;
        foreach (WebCamDevice oWebCamDevice in WebCamTexture.devices)
        {
            if (oWebCamDevice.isFrontFacing)
            {
                sDeviceName = oWebCamDevice.name;
                break;
            }
        }

        if (sDeviceName == String.Empty) return;

        m_oWebCamTexture = new WebCamTexture(sDeviceName);

        // Set up webcam texture
        RawImage oRawImage = WebCamGO.GetComponent<RawImage>();
        oRawImage.texture = m_oWebCamTexture;
        oRawImage.material.mainTexture = m_oWebCamTexture;
    }

    public void InitInstance() { }

    public void Tick()
    {

    }

    public void Play()
    {
        if (m_oWebCamTexture == null) return;

        WebCamGO.SetActive(true);
        m_oWebCamTexture.Play();
    }

    public void Stop()
    {
        if (m_oWebCamTexture == null) return;

        m_oWebCamTexture.Stop();
        WebCamGO.SetActive(false);
    }
}