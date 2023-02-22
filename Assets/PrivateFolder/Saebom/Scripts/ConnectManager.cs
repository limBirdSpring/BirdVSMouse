using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Saebom;
using SoYoon;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ConnectManager : MonoBehaviourPunCallbacks
{
    //public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    //{
    //    PlayGameManager.Instance.PlayerDie(PhotonNetwork.LocalPlayer.GetPlayerNumber());
    //    PhotonNetwork.LeaveRoom();
    //    Debug.Log(PhotonNetwork.PlayerList.Length);
    //}

    private void Start()
    {
        Hashtable props = new Hashtable()
        {
            { "Load" , true },
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Load"))
        {
            if (CheckPlayerLoaded())
            {
                PlayGameManager.Instance.GameStart();
            }
            else
            {
                Debug.Log("Waiting for another players");
            }
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Saebom.PlayGameManager.Instance.PlayerDie(otherPlayer.GetPlayerNumber());
        //ScoreManager.Instance.MasterCurPlayerStateUpdate();
        //ScoreManager.Instance.TurnResult();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(string.Format("접속 해제 : {0}", cause.ToString()));
        SceneManager.LoadScene("TitleTestScene");
    }

    public bool CheckPlayerLoaded()
    {
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            object isPlayerLoaded;
            if (player.CustomProperties.TryGetValue("Load", out isPlayerLoaded))
            {
                if (!(bool)isPlayerLoaded)
                    return false;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}
