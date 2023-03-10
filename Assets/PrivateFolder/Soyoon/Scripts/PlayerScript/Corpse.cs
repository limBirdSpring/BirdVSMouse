using Photon.Pun;
using Saebom;
using UnityEngine;

namespace SoYoon
{
    public class Corpse : MonoBehaviour
    {
        [HideInInspector]
        public int playerNum;

        private PhotonView targetPhotonView = null;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 활동시기인 동물들은 모두 발견 가능 -> 신고(투표창)
            // 비활동시기인 동물들은 시체를 없애기 가능
            //Debug.Log(collision.gameObject.name);
            if (!(collision.gameObject.name == PlayGameManager.Instance.myPlayerState.playerPrefab.name))
                return;

            //Debug.Log("On Collision");
            PlayerControllerTest controller = collision.GetComponent<PlayerControllerTest>();
            targetPhotonView = collision.gameObject.GetPhotonView();

            if (targetPhotonView.IsMine)
            {
                if (controller.state == PlayerState.Active)
                {
                    //Debug.Log("On active Collision");
                    InterActionAdapter adapter = gameObject.GetComponent<InterActionAdapter>();
                    adapter.OnInterAction.RemoveAllListeners();
                    adapter.OnInterAction.AddListener(FoundCorpse);
                }
                else if(controller.state == PlayerState.Inactive)
                {
                    //Debug.Log("On inactive Collision");
                    InterActionAdapter adapter = gameObject.GetComponent<InterActionAdapter>();
                    adapter.OnInterAction.RemoveAllListeners();
                    adapter.OnInterAction.AddListener(DestroyCorpse);
                }
            }
        }

        private void FoundCorpse()
        {
            targetPhotonView.RPC("FoundCorpse", RpcTarget.All, playerNum);
            targetPhotonView.RPC("DestroyCorpse", RpcTarget.All, playerNum, true);
            //Saebom.MissionButton.Instance.MissionButtonOff();
        }

        private void DestroyCorpse()
        {
            targetPhotonView.RPC("DestroyCorpse", RpcTarget.All, playerNum, false);
            //Saebom.MissionButton.Instance.MissionButtonOff();
        }
    }
}