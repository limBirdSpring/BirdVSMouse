using Photon.Pun;
using UnityEngine;

namespace SoYoon
{
    public class Corpse : MonoBehaviourPun
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 활동시기인 동물들은 모두 발견 가능 -> 신고(투표창)
            // 비활동시기인 동물들은 시체를 없애기 가능
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