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
            // ���� ��Ȳ������ kill ���� ���� ���ֱ�
            target?.transform.GetChild(1).gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
