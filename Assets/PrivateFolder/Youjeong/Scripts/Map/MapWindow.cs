using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWindow : MonoBehaviour
{
    //��ư�� ������ Ȱ��ȭ��ų �̼� â
    [SerializeField]
    private GameObject missionWindow;

    public void MissionWindowOn()
    {
        missionWindow.SetActive(true);
    }

    public void MissionWindowOff()
    {
        missionWindow.SetActive(false);
    }
}
