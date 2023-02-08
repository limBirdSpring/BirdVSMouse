using Photon.Pun;
using UnityEngine;

namespace SoYoon
{
    public class KillButton : MonoBehaviour
    {
        [HideInInspector]
        public GameObject target;

        public void OnClickedKillButton()
        {
            target?.GetComponent<PlayerControllerTest>().Die();
            target?.GetPhotonView().RPC("Die", RpcTarget.All);
            // 죽은 상황에서는 kill 범위 감지 꺼주기
            target?.transform.GetChild(1).gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
