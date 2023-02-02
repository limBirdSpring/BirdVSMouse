using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�ش� ������Ʈ�� �̼� ��ó �ݸ����� �߰����Ѽ� ����Ѵ�.

public class MissionOn : MonoBehaviour
{
    //��ư�� ������ Ȱ��ȭ��ų �̼� â
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
