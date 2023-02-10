using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;

public class RopeUpdate : Mission
{
    [SerializeField]
    private RopeController control;

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