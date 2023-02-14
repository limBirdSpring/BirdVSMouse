using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Saebom;
using SoYoon;
using UnityEngine;

public class ConnectManager : MonoBehaviourPunCallbacks
{
    //public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    //{
    //    PlayGameManager.Instance.PlayerDie(PhotonNetwork.LocalPlayer.GetPlayerNumber());
    //    PhotonNetwork.LeaveRoom();
    //    Debug.Log(PhotonNetwork.PlayerList.Length);
    //}

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Saebom.PlayGameManager.Instance.PlayerDie(otherPlayer.GetPlayerNumber());
        //ScoreManager.Instance.MasterCurPlayerStateUpdate();
        //ScoreManager.Instance.TurnResult();
    }
}
