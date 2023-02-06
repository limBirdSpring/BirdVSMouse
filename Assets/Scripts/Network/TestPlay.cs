using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlay : MonoBehaviourPunCallbacks
{

    private void Start()
    {
        if (!PhotonNetwork.InRoom)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions() { MaxPlayers = 12 }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");
    }

}
