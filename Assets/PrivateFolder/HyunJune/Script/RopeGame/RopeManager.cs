using Saebom;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class RopeManager : Mission
{
    private RopeGame[] ropeGames;

    [SerializeField]
    private SunOrMoon sun;
    [SerializeField]
    private SunOrMoon moon;

    public void ResetRope()
    {
        ropeGames = GetComponentsInChildren<RopeGame>();
        
        for (int i = 0; i < ropeGames.Length; i++)
        {
            ropeGames[i].RopeReset();
        }
    }
    public override void OnEnable()
    {
        base.OnEnable();
        GraphicUpdate();
    }

    public override void GraphicUpdate()
    {
        photonView.RPC("LoadUIRPC", RpcTarget.All, null);
    }

    [PunRPC]
    private void LoadUIRPC()
    {
        ropeGames = GetComponentsInChildren<RopeGame>();

        for (int i = 0; i < ropeGames.Length; i++)
        {
            ropeGames[i].UpdateUI();
        }
    }

    public override void PlayerUpdateCurMission()
    {
        ropeGames = GetComponentsInChildren<RopeGame>();

        photonView.RPC("SaveUIRPC", RpcTarget.All, ropeGames);
    }

    [PunRPC]
    private void SaveUIRPC(RopeGame[] ropes)
    {
        ropeGames = GetComponentsInChildren<RopeGame>();

        for (int i = 0; i < ropeGames.Length; i++)
        {
            ropeGames[i].curState = ropes[i].curState;
        }
    }

    public void SunOrMoonStart()
    {
        if (TimeManager.Instance.isCurNight)
        {
            moon.StartMove();
        }
        else
        {
            sun.StartMove();
        }
    }

    public override bool GetScore()
    {
        if (TimeManager.Instance.isCurNight)
        {
            moon.ResetPos();

            if (moon.MissionSuccess)
                return true;
            else
                return false;

        }
        else
        {
            sun.ResetPos();

            if (sun.MissionSuccess)
                return true;
            else
                return false;
        }
    }
}
