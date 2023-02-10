using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;
using Photon.Pun;
using Unity.VisualScripting;

public class RopeManager : Mission
{
    [SerializeField]
    private RopeController control;

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
        control.LoadUIRPC();
    }

    public override void PlayerUpdateCurMission()
    {
        RopeGame[] ropeGames = control.GetComponentsInChildren<RopeGame>();

        photonView.RPC("SaveUIRPC", RpcTarget.All, ropeGames);
    }

    [PunRPC]
    private void SaveUIRPC(RopeGame[] ropes)
    {
        control.SaveUIRPC(ropes);
    }
}
