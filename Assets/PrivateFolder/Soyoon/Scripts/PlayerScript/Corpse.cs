using Photon.Pun;
using Saebom;
using SoYoon;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    public void DestroyCorpse()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("KillRange")) // active state
        {
            // �������� ��� : �������� ����, �����̰� �ƴ� ��� ������
            if (collision.GetComponentInParent<PlayerControllerTest>() != null)
            {
                PlayerControllerTest controller = collision.GetComponentInParent<PlayerControllerTest>();
                PhotonView photonView = collision.GetComponentInParent<PhotonView>();
                if (!PlayGameManager.Instance.myPlayerState.isSpy && photonView.IsMine)
                    controller.FoundCorpse();
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerControllerTest controller = collision.GetComponent<PlayerControllerTest>();
            PhotonView photonView = collision.GetComponent<PhotonView>();
            if (controller.state != PlayerState.Inactive)
                Saebom.MissionButton.Instance.MissionButtonOff();
        }
    }
}