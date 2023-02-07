using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Saebom;

//�ش� ������Ʈ�� ������ ������Ʈ ��ó �ݸ����� �߰����Ѽ� ����Ѵ�.


public class ItemGet : MonoBehaviourPun
{
    //�ش� ��ҿ��� ���� ������
    [SerializeField]
    private ItemData itemData;

    [SerializeField]
    private GameObject blockButton;

    private float waitingTime =0;

    public void GetItem()
    {
        if ((PlayGameManager.Instance.myPlayerState.isBird && !TimeManager.Instance.isCurNight) ||
            (!PlayGameManager.Instance.myPlayerState.isBird && TimeManager.Instance.isCurNight))
        {
            //�����ư ��Ƽ��
            blockButton.SetActive(true);
            StartCoroutine(ItemGetActiveBlockButton());
        }
        else
        {
            if (PlayGameManager.Instance.myPlayerState.isSpy)
            {
                if (waitingTime == 0)
                    photonView.RPC("ItemHindrance", RpcTarget.All, 3);
            }
            //���� �ù��� ��쿣 ����
            else
            {
                if (waitingTime == 3)
                    photonView.RPC("ItemHindrance", RpcTarget.All, 0);
            }
        }
    }

    private IEnumerator ItemGetActiveBlockButton()
    {
        yield return new WaitForSeconds(waitingTime);
        Inventory.Instance.SetItem(itemData);
        //�����ư ��Ƽ�� �޽�
        blockButton.SetActive(false);
    }

    [PunRPC]
    public void ItemHindrance(float waitTime)
    {
        waitingTime = waitTime;
        StartCoroutine(ItemHimdranceEnd());
    }
    
    private IEnumerator ItemHimdranceEnd()
    {
        yield return new WaitForSeconds(2);
        waitingTime = 0;
    }
}
