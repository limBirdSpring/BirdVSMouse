using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Saebom;

//해당 컴포넌트는 아이템 오브젝트 근처 콜리더에 추가시켜서 사용한다.


public class ItemGet : MonoBehaviour
{
    //해당 장소에서 얻을 아이템
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
            //만약 방해 성공하면 방해당한 플레이어에게 방해성공 띄우기

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
        //블락버튼 액티브 펄스
        blockButton.SetActive(false);
    }

    [PunRPC]
    private void ItemHindrance(float waitTime)
    {
        Debug.Log("아이템 방해가 성공적입니다.");
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
