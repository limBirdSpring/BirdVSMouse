using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//해당 컴포넌트는 미션 근처 콜리더에 추가시켜서 사용한다.

public class MissionOn : MonoBehaviour
{
    //버튼을 누르면 활성화시킬 미션 창
    [SerializeField]
    private GameObject missionWindow;

    public bool active;

    public void MissionWindowOn()
    {
        active = true;
        missionWindow.SetActive(true);
    }

    public void MissionWindowOff()
    {
        missionWindow.SetActive(false);
        active = false;
    }
}
