using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�ش� ������Ʈ�� �̼� ��ó �ݸ����� �߰����Ѽ� ����Ѵ�.

public class BakMission : MonoBehaviour
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
