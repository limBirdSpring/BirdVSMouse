using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;
using Photon.Pun;
using Unity.VisualScripting;

public class RopeManager : Mission
{
    [SerializeField]
    private PhotonView photon;

    [SerializeField]
    private RopeController control;

    public int[] ropeGamesCurState;

    private void Start()
    {
        SetRopeGamesCurState(control.ropeGames);
    }

    public void GetRopeGamesCurState(RopeGame[] ropeGames )
    {
        for(int i=0; i< ropeGames.Length; i++)
        {
            switch(ropeGamesCurState[i])
            {
                case 0:
                    ropeGames[i].curState = RopeState.None;
                    break;
                case 1:
                    ropeGames[i].curState = RopeState.Rot;
                    break;
                case 2:
                    ropeGames[i].curState = RopeState.Normal;
                    break;
            }
        }
    }

    public void SetRopeGamesCurState(RopeGame[] ropeGames)
    {
        for (int i = 0; i < ropeGames.Length; i++)
        {
            switch (ropeGames[i].curState)
            {
                case RopeState.None:
                    
                    break;
                case RopeState.Rot:
                    ropeGamesCurState[i] = 1;
                    break;
                case RopeState.Normal:
                    ropeGamesCurState[i] = 2;
                    break;
            }
        }
    }

    public override bool GetScore()
    {
        if (TimeManager.Instance.isCurNight)
        {
            control.moon.ResetPos();

            if (control.moon.MissionSuccess)
                return true;
            else
                return false;

        }
        else
        {
            control.sun.ResetPos();

            if (control.sun.MissionSuccess)
                return true;
            else
                return false;
        }
    }

    public override void PlayerUpdateCurMission()
    {
        photon.RPC("SaveUIRPC", RpcTarget.All, (int[])ropeGamesCurState);
    }
}

