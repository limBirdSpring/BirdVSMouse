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
        // 지금이 밤 일떄
        if (TimeManager.Instance.isCurNight)
        {
            if (cloth.curColor == MissionButton.Instance.mouseMission.color)
            {
                // 미션 성공
                return true;
            }
            else
            {
                // 미션 실패
                return false;
            }
        }
        // 낮일 때
        else
        {
            if (cloth.curColor == MissionButton.Instance.birdMission.color)
            {
                // 미션 성공
                return true;
            }
            else
            {
                // 미션 실패
                return false;
            }
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PlayerUpdateCurMission();
    }

   
    public override void PlayerUpdateCurMission()
    {
        photon.RPC("ClothCurColorRPC", RpcTarget.All, cloth.curColor);
    }

}
