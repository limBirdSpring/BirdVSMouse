using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InterActionAdapter : MonoBehaviour
{
    public UnityEvent OnInterAction = null;

    [HideInInspector]
    public bool isActive = false;

    public void Interaction()
    {
        OnInterAction?.Invoke();
    }
}