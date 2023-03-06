using Cinemachine;
using Photon.Pun;
using Saebom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nulttuigi : MonoBehaviourPun
{
    [SerializeField]
    private CinemachineVirtualCamera cam;

    [SerializeField]
    private GameObject missionWindow;

   
    public void Jump()
    {
        GetComponent<AudioSource>().Play();
         missionWindow.SetActive(true);
        gameObject.GetComponent<InterActionAdapter>().isActive = true;
        photonView.RPC("NulttuigiActiveUpdate", RpcTarget.All, true);
        PlayGameManager.Instance.myPlayerState.playerPrefab.GetComponent<FieldOfView>().InNulttuigi();
        cam.Priority = 30;
        StartCoroutine(JumpCor());
    }

    private IEnumerator JumpCor()
    {
        yield return new WaitForSeconds(2f);
        cam.Priority = 1;
        gameObject.GetComponent<InterActionAdapter>().isActive = false;
        photonView.RPC("NulttuigiActiveUpdate", RpcTarget.All, false);
        missionWindow.SetActive(false);
        // 널뛰기 한 후 땅에 도착할 떄 카메라 컬링 마스크 원래대로
        yield return new WaitForSeconds(2f);
        PlayGameManager.Instance.myPlayerState.playerPrefab.GetComponent<FieldOfView>().OutNulttuigi();
    }

    [PunRPC]
    public void NulttuigiActiveUpdate(bool active)
    {
        gameObject.GetComponent<InterActionAdapter>().isActive = active;
        //MissionButton.Instance.MissionButtonOn();
    }

}
