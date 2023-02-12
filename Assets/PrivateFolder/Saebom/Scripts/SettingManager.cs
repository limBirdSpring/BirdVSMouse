using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : SingleTon<SettingManager>
{
    //�̸����� Ƚ��
    public int emergencyCount = 3;

    //�ð��� ������ �帣�� ���� �ɸ��� ���ؼ��� Ƚ��
    public int successHiderance = 5;
    private int curHiderance = 5;

    //���� �� ������ ų ��
    public int killCount = 1;

    //�� �ϴ� �� �ð�
    public float turnTime = 360f;

    //���� Ƚ�� ����
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
