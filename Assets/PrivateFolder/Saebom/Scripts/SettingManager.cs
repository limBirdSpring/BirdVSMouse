using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : SingleTon<SettingManager>
{
    //�̸����� Ƚ�� : 3,4,5
    public int emergencyCount = 3;

    //�ð��� ������ �帣�� ���� �ɸ��� ���ؼ��� Ƚ�� : 3,5,7
    public int successHiderance = 5;
    private int curHiderance = 5;

    //���� �� ������ ų �� : 1,2
    public int killCount = 1;

    //�� �ϴ� �� �ð� : 360, 500, 700, 900
    public float turnTime = 360f;

    //���� Ƚ�� ���� : 8, 10, 12
    public int maxRoundCount = 8;

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
    }

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
