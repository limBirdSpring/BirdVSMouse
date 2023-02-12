using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : SingleTon<SettingManager>
{
    //이멀젼시 횟수
    public int emergencyCount = 3;

    //시간이 빠르게 흐르는 데에 걸리는 방해성공 횟수
    public int successHiderance = 5;
    private int curHiderance = 5;

    //한판 당 한정할 킬 수
    public int killCount = 1;

    //한 턴당 총 시간
    public float turnTime = 360f;

    //라운드 횟수 제한
    public int maxRoundCount = 8;

    public void OnEmergencyCountButtonClick(int count)
    {
        emergencyCount = count;
    }

    public void OnHideranceCountButtonClick(int count)
    {
        successHiderance = curHiderance = count;
    }

    public void OnKillCountButtonClick(int count)
    {
        killCount = count;
    }

    public void OnTurnTimeButtonClick(float time)
    {
        turnTime = time;
    }

    public void OnMaxRoundCountClick(int count)
    {
        maxRoundCount = count;
    }

}
