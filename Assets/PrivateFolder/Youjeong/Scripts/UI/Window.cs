using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    //��ư�� ������ Ȱ��ȭ��ų �̼� â
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
