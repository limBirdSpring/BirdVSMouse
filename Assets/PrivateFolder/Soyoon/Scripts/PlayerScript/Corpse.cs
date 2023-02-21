using Photon.Pun;
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
            // Ȱ���ñ��� �������� ��� �߰� ���� -> �Ű�(��ǥâ)
            // ��Ȱ���ñ��� �������� ��ü�� ���ֱ� ����
            //Debug.Log(collision.gameObject.name);
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("InActive"))
            {
                //Debug.Log("On Collision");
                PlayerControllerTest controller = collision.GetComponent<PlayerControllerTest>();
                targetPhotonView = collision.GetComponent<PhotonView>();

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
        }

        //private void OnTriggerExit2D(Collider2D collision)
        //{
        //    if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("InActive"))
        //    {
        //        if (targetPhotonView.IsMine)
        //            Saebom.MissionButton.Instance.MissionButtonOff();
        //    }
        //}

        private void FoundCorpse()
        {
            targetPhotonView.RPC("FoundCorpse", RpcTarget.All, playerNum);
            targetPhotonView.RPC("DestroyCorpse", RpcTarget.All, playerNum, true);
            //Saebom.MissionButton.Instance.MissionButtonOff();
        }

        private void DestroyCorpse()
        {
            targetPhotonView.RPC("DestroyCorpse", RpcTarget.All, playerNum);
            //Saebom.MissionButton.Instance.MissionButtonOff();
        }
    }
}