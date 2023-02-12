using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Saebom;

//�ش� ������Ʈ�� ������ ������Ʈ ��ó �ݸ����� �߰����Ѽ� ����Ѵ�.


public class ItemGet : MonoBehaviour
{
    //�ش� ��ҿ��� ���� ������
    [SerializeField]
    private ItemData itemData;

    [SerializeField]
    private GameObject clock;

    [SerializeField]
    private GameObject hiderancePrefab;

    [SerializeField]
    private GameObject blockButton;

    private PhotonView photonView;

    private float waitingTime =0;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void GetItem()
    {
        if ((PlayGameManager.Instance.myPlayerState.isBird && !TimeManager.Instance.isCurNight) ||
            (!PlayGameManager.Instance.myPlayerState.isBird && TimeManager.Instance.isCurNight))
        {
            //���� ���� �����ϸ� ���ش��� �÷��̾�� ���ؼ��� ����

            if (waitingTime == 3f)
            {
                Instantiate(hiderancePrefab, clock.transform.position, Quaternion.identity);
            }

            blockButton.SetActive(true);
            StartCoroutine(ItemGetActiveBlockButton());
        }
        else
        {
            if (waitingTime == 0)
            {
                clock.SetActive(true);
                photonView.RPC("ItemHindrance", RpcTarget.All, 3f);
                StartCoroutine(ItemHimdranceEnd());
            }
            
        }
    }

    private IEnumerator ItemGetActiveBlockButton()
    {
        yield return new WaitForSeconds(waitingTime);
        SoundManager.Instance.PlayUISound(UISFXName.ItemGet);
        Inventory.Instance.SetItem(itemData);
        //�����ư ��Ƽ�� �޽�
        blockButton.SetActive(false);
    }

    [PunRPC]
    private void ItemHindrance(float waitTime)
    {
        Debug.Log("������ ���ذ� �������Դϴ�.");
        waitingTime = waitTime;
       
    }
    
    private IEnumerator ItemHimdranceEnd()
    {
        yield return new WaitForSeconds(2);
        waitingTime = 0;
        clock.SetActive(false);
        photonView.RPC("ItemHindrance", RpcTarget.All, 0f);
    }
}
