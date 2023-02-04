using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
        //만약 활동시간이라면
        int random = Random.Range(0, max);

        if (random ==0)
            Inventory.Instance.SetItem(normalRope);
        else
            Inventory.Instance.SetItem(rotRope);

        //만약 방해시간이라면

        //내가 스파이일 경우엔 무조건 획득으로 도와줌

        //내가 시민일 경우엔 방해
        if (max ==2)
            photonView.RPC("Hindrance", RpcTarget.All);

    }

    [PunRPC]
    public void Hindrance()
    {
        max = 5;

        StartCoroutine(HindCor());
    }

    private IEnumerator HindCor()
    {
        yield return new WaitForSeconds(3f);
        max = 2;
    }

}
