using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor.UI;
using UnityEditor.U2D;
using Unity.VisualScripting;

public class InterActionAdapter : MonoBehaviour
{
    public UnityEvent OnInterAction = null;

    [HideInInspector]
    public bool isActive = false;

    [SerializeField]
    private GameObject outLine = null;

    public void Interaction()
    {
        OnInterAction?.Invoke();
    }

    public void OutLineOn()
    {
        if (outLine != null)
            outLine.SetActive(true);
    }


    public void OutLineOff()
    {
        if (outLine != null)
            outLine.SetActive(false);
    }
}