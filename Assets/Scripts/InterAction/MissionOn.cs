using Photon.Pun;
using Saebom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�ش� ������Ʈ�� �̼� ��ó �ݸ����� �߰����Ѽ� ����Ѵ�.

public class MissionOn : MonoBehaviourPun
{
    //��ư�� ������ Ȱ��ȭ��ų �̼� â
    [SerializeField]
    private GameObject missionWindow;

    public void MissionWindowOn()
    {
        if ((PlayGameManager.Instance.myPlayerState.isBird && !TimeManager.Instance.isCurNight) ||
           (!PlayGameManager.Instance.myPlayerState.isBird && TimeManager.Instance.isCurNight))
        {
            missionWindow.SetActive(true);
            gameObject.GetComponent<InterActionAdapter>().isActive = true;
            photonView.RPC("MissionWindowActiveUpdate", RpcTarget.All, true);
        }

    }

    public void MissionWindowOff()
    {
        if ((PlayGameManager.Instance.myPlayerState.isBird && !TimeManager.Instance.isCurNight) ||
            (!PlayGameManager.Instance.myPlayerState.isBird && TimeManager.Instance.isCurNight))
        {
            missionWindow.SetActive(false);
            gameObject.GetComponent<InterActionAdapter>().isActive = false;
            photonView.RPC("MissionWindowActiveUpdate", RpcTarget.All, false);
        }
    }

    [PunRPC]
    public void MissionWindowActiveUpdate(bool active)
    {
        gameObject.GetComponent<InterActionAdapter>().isActive = active;
        MissionButton.Instance.MissionButtonOn();
    }

}
