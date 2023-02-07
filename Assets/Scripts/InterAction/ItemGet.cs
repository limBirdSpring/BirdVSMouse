using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Saebom;

//해당 컴포넌트는 아이템 오브젝트 근처 콜리더에 추가시켜서 사용한다.


public class ItemGet : MonoBehaviourPun
{
    //해당 장소에서 얻을 아이템
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
            //블락버튼 액티브
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
            //내가 시민일 경우엔 방해
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
        //블락버튼 액티브 펄스
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
