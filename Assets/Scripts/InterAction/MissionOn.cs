using Photon.Pun;
using Saebom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//해당 컴포넌트는 미션 근처 콜리더에 추가시켜서 사용한다.

public class MissionOn : MonoBehaviourPun
{
    //버튼을 누르면 활성화시킬 미션 창
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
