using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWindow : MonoBehaviour
{
    //버튼을 누르면 활성화시킬 미션 창
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
