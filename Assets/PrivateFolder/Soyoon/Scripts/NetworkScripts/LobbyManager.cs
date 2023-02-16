using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace SoYoon
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        public static LobbyManager Instance { get; private set; }

        private AudioSource lobbyBGM;

        private void Awake()
        {
            if (GameObject.Find("LobbyManager") != null)
            {
                LobbyManager[] lobby = GameObject.FindObjectsOfType<LobbyManager>();
                if (lobby.Length == 1)
                {
                    Instance = this;
                    DontDestroyOnLoad(this);
                }
                else
                    Destroy(this.gameObject);
            }

            lobbyBGM = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (PhotonNetwork.InRoom)
            {
                Debug.Log("inRoom");
                OnJoinedRoom();
            }
            else if (PhotonNetwork.IsConnected)
            {
                Debug.Log("isConnected");
                OnConnectedToMaster();
            }
            else
            {
                // 접속 해제 되었을 경우 if(!PhotonNetwork.IsConnected)
                OnDisconnected(DisconnectCause.None);
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("접속 완료");

            if (SceneManager.GetActiveScene().name == "RoomTestScene")
                SceneManager.LoadScene("LobbyTestScene");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log(string.Format("접속 해제 : {0}", cause.ToString()));
            SceneManager.LoadScene("TitleTestScene");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("방 접속 완료");
            SceneManager.LoadScene("RoomTestScene");
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.CurrentRoom.IsVisible = true;
            if (!lobbyBGM.isPlaying)
                lobbyBGM.Play();

            if (PhotonNetwork.IsMasterClient)
            {
                // master는 ready한 상태로 방에 입장
                Hashtable props = new Hashtable()
                {
                    { "Ready", true },
                    { "Load" , false },
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            }
            else
            {
                Hashtable props = new Hashtable()
                {
                    { "Ready", false },
                    { "Load" , false },
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            }

            RoomPanel roomPanel = GameObject.Find("Canvas").GetComponent<RoomPanel>();
            roomPanel?.UpdateRoomState();
            roomPanel?.UpdateLocalPlayerPropertiesUpdate();
        }

        public override void OnLeftRoom()
        {
            Debug.Log("방 나가기 완료");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("플레이어 들어옴");
            RoomPanel roomPanel = GameObject.Find("Canvas").GetComponent<RoomPanel>();
            roomPanel?.UpdateRoomState();
            roomPanel?.UpdateLocalPlayerPropertiesUpdate();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("플레이어 나감");
            RoomPanel roomPanel = GameObject.Find("Canvas").GetComponent<RoomPanel>();
            roomPanel?.UpdateRoomState();
            roomPanel?.UpdateLocalPlayerPropertiesUpdate();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            Debug.Log("방장 변경");
            RoomPanel roomPanel = GameObject.Find("Canvas").GetComponent<RoomPanel>();
            roomPanel?.UpdateRoomState();
            roomPanel?.UpdateLocalPlayerPropertiesUpdate();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            Debug.Log("플레이어 상태 변화");

            if(targetPlayer == PhotonNetwork.LocalPlayer)
            {
                if (changedProps["IsKicked"] != null)
                {
                    if ((bool)changedProps["IsKicked"])
                    {
                        string[] removeProperties = new string[1];
                        removeProperties[0] = "isKicked";
                        PhotonNetwork.RemovePlayerCustomProperties(removeProperties);
                        PhotonNetwork.LeaveRoom();
                    }
                }
            }

            RoomPanel roomPanel = GameObject.Find("Canvas").GetComponent<RoomPanel>();
            roomPanel?.UpdateRoomState();
            roomPanel?.UpdateLocalPlayerPropertiesUpdate();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("로비 입장 완료");
            if(!lobbyBGM.isPlaying)
                lobbyBGM.Play();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("방 정보 갱신");
            LobbyPanel lobbyPanel = GameObject.Find("Canvas").transform.GetChild(3).GetComponent<LobbyPanel>();
            lobbyPanel?.UpdateRoomList(roomList);
        }

        public override void OnLeftLobby()
        {
            Debug.Log("로비 나감");
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
