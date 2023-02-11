using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;
using Unity.Mathematics;
using HyunJune;
using Cloth = HyunJune.Cloth;
using Photon.Pun;

public class DyeManager : Mission
{
    [SerializeField]
    private PhotonView photon;

    [SerializeField]
    public Cloth cloth;

    public override bool GetScore()
    {
        // ������ �� �ϋ�
        if (TimeManager.Instance.isCurNight)
        {
            if (cloth.curColor == MissionButton.Instance.mouseMission.color)
            {
                // �̼� ����
                return true;
            }
            else
            {
                // �̼� ����
                return false;
            }
        }
        // ���� ��
        else
        {
            if (cloth.curColor == MissionButton.Instance.birdMission.color)
            {
                // �̼� ����
                return true;
            }
            else
            {
                // �̼� ����
                return false;
            }
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        GraphicUpdate();
    }

    public override void OnDisable()
    {
        PlayerUpdateCurMission();
        base.OnDisable();
    }

    public override void GraphicUpdate()
    {
        cloth.clothImage.color = cloth.dicColor[cloth.curColor];
    }

    public override void PlayerUpdateCurMission()
    {
        photon.RPC("ClothCurColorRPC", RpcTarget.All, cloth.curColor);
    }

}
