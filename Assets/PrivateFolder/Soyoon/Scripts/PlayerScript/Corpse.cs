using Photon.Pun;
using Saebom;
using SoYoon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //PlayGameManager playGameManager = GameObject.Find("PlayGameManager").GetComponent<PlayGameManager>();
        if(collision.gameObject.name == "KillRangeCollider") // active state
        {
            // �������� ��� : �������� ����, �����̰� �ƴ� ��� ������
            if(collision.GetComponentInParent<PlayerControllerTest>() != null)
            {
                PlayerControllerTest controller = collision.GetComponentInParent<PlayerControllerTest>();
                PhotonView photonView = collision.GetComponentInParent<PhotonView>();
                if (!PlayGameManager.Instance.myPlayerState.isSpy && photonView.IsMine)
                    controller.FoundCorpse();
            }
        }

        if(collision.GetComponent<PlayerControllerTest>() != null)
        {
            PlayerControllerTest controller = collision.GetComponentInParent<PlayerControllerTest>();
            PhotonView photonView = collision.GetComponentInParent<PhotonView>();
            if (controller.state == PlayerState.Inactive)
                controller.FoundCorpse();
        }
    }
}