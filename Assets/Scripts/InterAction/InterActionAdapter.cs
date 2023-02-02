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
    private GameObject outLine;

    public void Interaction()
    {
        OnInterAction?.Invoke();
    }

    public void OutLineOn()
    {
        outLine.SetActive(true);
    }


    public void OutLineOff()
    {
        outLine.SetActive(false);
    }
}