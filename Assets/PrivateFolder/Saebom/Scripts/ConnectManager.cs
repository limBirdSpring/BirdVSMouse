using Photon.Pun;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectManager : MonoBehaviourPunCallbacks
{
    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    { 
        PhotonNetwork.LeaveRoom();
        Debug.Log(PhotonNetwork.PlayerList.Length);
    }
}
