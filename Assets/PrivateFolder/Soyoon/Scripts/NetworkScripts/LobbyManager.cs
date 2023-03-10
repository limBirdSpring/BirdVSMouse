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
                // 立加 秦力 登菌阑 版快 if(!PhotonNetwork.IsConnected)
                OnDisconnected(DisconnectCause.None);
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("立加 肯丰");

            if (SceneManager.GetActiveScene().name == "RoomTestScene")
                SceneManager.LoadScene("LobbyTestScene");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log(string.Format("立加 秦力 : {0}", cause.ToString()));
            SceneManager.LoadScene("TitleTestScene");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("规 立加 肯丰");
            SceneManager.LoadScene("RoomTestScene");
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.CurrentRoom.IsVisible = true;
            if (!lobbyBGM.isPlaying)
                lobbyBGM.Play();

            if (PhotonNetwork.IsMasterClient)
            {
                // master绰 ready茄 惑怕肺 规俊 涝厘
                Hashtable props = new Hashtable()
                {
                    { "Ready", true },
                    { "Load" , false },
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                Hashtable roomProps = new Hashtable();
                roomProps.Add("AbleToStartGame", true);
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
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
            Debug.Log("规 唱啊扁 肯丰");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("敲饭捞绢 甸绢咳");
            RoomPanel roomPanel = GameObject.Find("Canvas").GetComponent<RoomPanel>();
            roomPanel?.UpdateRoomState();
            roomPanel?.UpdateLocalPlayerPropertiesUpdate();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("敲饭捞绢 唱皑");
            RoomPanel roomPanel = GameObject.Find("Canvas").GetComponent<RoomPanel>();
            roomPanel?.UpdateRoomState();
            roomPanel?.UpdateLocalPlayerPropertiesUpdate();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            Debug.Log("规厘 函版");
            RoomPanel roomPanel = GameObject.Find("Canvas").GetComponent<RoomPanel>();
            roomPanel?.UpdateRoomState();
            roomPanel?.UpdateLocalPlayerPropertiesUpdate();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            Debug.Log("敲饭捞绢 惑怕 函拳");

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

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if(propertiesThatChanged.ContainsKey("AbleToStartGame"))
            {
                if ((bool)propertiesThatChanged["AbleToStartGame"])
                {
                    object alreadyReady;
                    if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Ready", out alreadyReady))
                        if ((bool)alreadyReady)
                            PhotonNetwork.AutomaticallySyncScene = true;
                }
            }
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("肺厚 涝厘 肯丰");
            if(!lobbyBGM.isPlaying)
                lobbyBGM.Play();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("规 沥焊 盎脚");
            LobbyPanel lobbyPanel = GameObject.Find("Canvas").transform.GetChild(3).GetComponent<LobbyPanel>();
            lobbyPanel?.UpdateRoomList(roomList);
        }

        public override void OnLeftLobby()
        {
            Debug.Log("肺厚 唱皑");
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log(string.Format("规 积己 角菩 {0} : {1}", returnCode, message));
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log(string.Format("规 立加 角菩 {0} : {1}", returnCode, message));
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log(string.Format("罚待 规 立加 角菩 {0} : {1}", returnCode, message));
            Debug.Log("规阑 积己秦辑 立加钦聪促.");

            // TODO : 规 捞抚 罚待 积己
            string roomName = string.Format("Room {0}", Random.Range(1000, 10000));
            RoomOptions options = new RoomOptions() { MaxPlayers = (byte)12 };
            PhotonNetwork.CreateRoom(roomName, options);

            SceneManager.LoadScene("RoomTestScene");
        }
    }
}
