using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFullscreen : MonoBehaviour
{
    void Awake()
    {
        RectTransform oRT = gameObject.GetComponent<RectTransform>();
        oRT.sizeDelta = new Vector2(Screen.width, Screen.height);
        oRT.localPosition = Vector3.zero;
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
