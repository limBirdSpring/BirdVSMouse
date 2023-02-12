using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;

public class RopeUpdate : Mission
{
    [SerializeField]
    private PhotonView photon;

    [SerializeField]
    private RopeController control;

    public override void OnDisable()
    {
        base.OnDisable();
        PlayerUpdateCurMission();
    }

    public override void PlayerUpdateCurMission()
    {
        RopeGame[] ropeGames = control.GetComponentsInChildren<RopeGame>();

        photon.RPC("SaveUIRPC", RpcTarget.All, ropeGames);
    }
}
