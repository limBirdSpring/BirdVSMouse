using Saebom;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public override void GraphicUpdate()
    {
        photonView.RPC("LoadUIRPC", RpcTarget.All, null);
    }
    
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

    private void SaveUIRPC(RopeGame[] ropes)
    {
        ropeGames = GetComponentsInChildren<RopeGame>();

        for (int i = 0; i < ropeGames.Length; i++)
        {
            ropeGames[i].rope = ropes[i].rope;
        }
    }
}
