using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�ش� ������Ʈ�� �̼� ��ó �ݸ����� �߰����Ѽ� ����Ѵ�.

public class MissionOn : MonoBehaviour
{
    //��ư�� ������ Ȱ��ȭ��ų �̼� â
    [SerializeField]
    private GameObject missionWindow;

    public void MissionWindowOn()
    {
        missionWindow.SetActive(true);
        gameObject.GetComponent<InterActionAdapter>().isActive = true;
    }

    public void MissionWindowOff()
    {
        missionWindow.SetActive(false);
        gameObject.GetComponent<InterActionAdapter>().isActive = false;
    }
}
