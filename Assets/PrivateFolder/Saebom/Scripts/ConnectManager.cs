using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Saebom;
using SoYoon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectManager : MonoBehaviourPunCallbacks
{
    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        PlayGameManager.Instance.PlayerDie(PhotonNetwork.LocalPlayer.GetPlayerNumber());
        PhotonNetwork.LeaveRoom();
        Debug.Log(PhotonNetwork.PlayerList.Length);
    }

  //  public override void OnPlayerLeftRoom(Player otherPlayer)
  //  {
  //      Debug.Log(PhotonNetwork.PlayerList.Length);
  //  }
}
