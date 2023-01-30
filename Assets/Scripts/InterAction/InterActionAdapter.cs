using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InterActionAdapter : MonoBehaviour
{
    public UnityEvent OnInterAction = null;

    public void Interaction()
    {
        OnInterAction?.Invoke();
    }
}