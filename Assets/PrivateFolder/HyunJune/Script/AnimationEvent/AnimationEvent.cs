using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public UnityEvent OnSwitchEvent;
    public UnityEvent OnTextEvent;

    public void ImageSwitch()
    {
        OnSwitchEvent?.Invoke();
    }

    public void OnText()
    {
        OnTextEvent?.Invoke();
    }
}
