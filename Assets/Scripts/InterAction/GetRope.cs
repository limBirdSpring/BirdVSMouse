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
        //���� Ȱ���ð��̶��
        int random = Random.Range(0, max);

        if (random ==0)
            Inventory.Instance.SetItem(normalRope);
        else
            Inventory.Instance.SetItem(rotRope);

        //���� ���ؽð��̶��

        //���� �������� ��쿣 ������ ȹ������ ������

        //���� �ù��� ��쿣 ����
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
