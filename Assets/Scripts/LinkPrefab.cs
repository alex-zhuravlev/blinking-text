using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkPrefab : MonoBehaviour
{
    [SerializeField]
    GameObject Prefab = null;

    GameObject m_oGO = null;

    private void Awake()
    {
        if (Prefab != null)
        {
            m_oGO = GameObject.Instantiate(Prefab);
            m_oGO.transform.SetParent(gameObject.transform);
        }
    }
}
