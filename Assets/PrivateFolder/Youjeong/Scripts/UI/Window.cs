using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    //버튼을 누르면 활성화시킬 미션 창
    [SerializeField]
    private GameObject window;

    public void WindowOn()
    {
        window.SetActive(true);
    }

    public void WindowOff()
    {
        window.SetActive(false);
    }
}
