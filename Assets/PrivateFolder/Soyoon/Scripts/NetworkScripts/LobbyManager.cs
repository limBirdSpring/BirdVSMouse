using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace SoYoon
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        public static LobbyManager Instance { get; private set; }

        private LobbyPanel lobbyPanel;

        private void Awake()
        {
            Instance = this;
            lobbyPanel = GameObject.Find("Canvas").transform.GetChild(3).GetComponent<LobbyPanel>();
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            if (PhotonNetwork.IsConnected)
            {
                OnJoinedLobby();
            }
            else if (PhotonNetwork.InRoom)
            {
                OnJoinedRoom();
            }
            else
            {
                // 접속 해제 되었을 경우 if(!PhotonNetwork.IsConnected)
                OnDisconnected(DisconnectCause.None);
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log(string.Format("접속 해제 : {0}", cause.ToString()));
            SceneManager.LoadScene("TitleTestScene");
        }

        public override void OnJoinedRoom()
        {
            SceneManager.LoadScene("RoomTestScene");
            Debug.Log("방 접속 완료");

            Hashtable props = new Hashtable()
            {
                { "Ready", false },
                { "Load" , false },
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            // TODO : UPDATE ROOM STATE

        }

        public override void OnJoinedLobby()
        {
            Debug.Log("로비 입장 완료");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("방 정보 갱신");
            lobbyPanel?.UpdateRoomList(roomList);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log(string.Format("방 생성 실패 {0} : {1}", returnCode, message));
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log(string.Format("방 접속 실패 {0} : {1}", returnCode, message));
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log(string.Format("랜덤 방 접속 실패 {0} : {1}", returnCode, message));
            Debug.Log("방을 생성해서 접속합니다.");

            // TODO : 방 이름 랜덤 생성
            string roomName = string.Format("Room {0}", Random.Range(1000, 10000));
            RoomOptions options = new RoomOptions() { MaxPlayers = (byte)12 };
            PhotonNetwork.CreateRoom(roomName, options);

            SceneManager.LoadScene("RoomTestScene");
        }
    }
}
