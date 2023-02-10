using Photon.Pun;
using UnityEngine;

namespace SoYoon
{
    public class Corpse : MonoBehaviourPun
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Ȱ���ñ��� �������� ��� �߰� ���� -> �Ű�(��ǥâ)
            // ��Ȱ���ñ��� �������� ��ü�� ���ֱ� ����
            //Debug.Log(collision.gameObject.name);
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("InActive"))
            {
                //Debug.Log("On Collision");
                PlayerControllerTest controller = collision.GetComponent<PlayerControllerTest>();
                PhotonView photonView = collision.GetComponent<PhotonView>();

                if (photonView.IsMine)
                {
                    if (controller.state == PlayerState.Active)
                    {
                        //Debug.Log("On active Collision");
                        InterActionAdapter adapter = gameObject.GetComponent<InterActionAdapter>();
                        adapter.OnInterAction.RemoveAllListeners();
                        adapter.OnInterAction.AddListener(controller.FoundCorpse);
                        Saebom.MissionButton.Instance.inter = adapter;
                        Saebom.MissionButton.Instance.MissionButtonOn();
                    }
                    else if(controller.state == PlayerState.Inactive)
                    {
                        //Debug.Log("On inactive Collision");
                        InterActionAdapter adapter = gameObject.GetComponent<InterActionAdapter>();
                        adapter.OnInterAction.RemoveAllListeners();
                        adapter.OnInterAction.AddListener(DestroyCorpse);
                        Saebom.MissionButton.Instance.inter = adapter;
                        Saebom.MissionButton.Instance.MissionButtonOn();
                    }
                }
            }
        }

        public void DestroyCorpse()
        {
            PhotonNetwork.Destroy(this.gameObject);
            Saebom.MissionButton.Instance.MissionButtonOff();
        }
    }
}