using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Saebom;

public class GetRope : MonoBehaviour
{
    [SerializeField]
    private ItemData normalRope;
    [SerializeField]
    private ItemData rotRope;

    PhotonView photonView;

    public int max=2;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void Get()
    {
        //���� Ȱ���ð��̶��
        if ((PlayGameManager.Instance.myPlayerState.isBird && !TimeManager.Instance.isCurNight) ||
            (!PlayGameManager.Instance.myPlayerState.isBird && TimeManager.Instance.isCurNight))
        {
            int random = Random.Range(0, max);

            if (random == 0)
                Inventory.Instance.SetItem(normalRope);
            else
                Inventory.Instance.SetItem(rotRope);
        }

        //���� ���ؽð��̶��
        else
        {
            //���� �������� ��쿣 ������ ȹ������ ������
            if (PlayGameManager.Instance.myPlayerState.isSpy)
            {
                if (max == 2)
                    photonView.RPC("Hindrance", RpcTarget.All, 1);
            }
            //���� �ù��� ��쿣 ����
            else
            {
                if (max == 2)
                    photonView.RPC("Hindrance", RpcTarget.All, 5);
            }
        }
    }

    [PunRPC]
    public void Hindrance(int max)
    {
        this.max = max;

        StartCoroutine(HindCor());
    }

    private IEnumerator HindCor()
    {
        yield return new WaitForSeconds(3f);
        max = 2;
    }

}
