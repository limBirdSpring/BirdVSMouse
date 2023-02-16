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
                // ���� ���� �Ǿ��� ��� if(!PhotonNetwork.IsConnected)
                OnDisconnected(DisconnectCause.None);
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("���� �Ϸ�");

            if (SceneManager.GetActiveScene().name == "RoomTestScene")
                SceneManager.LoadScene("LobbyTestScene");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log(string.Format("���� ���� : {0}", cause.ToString()));
            SceneManager.LoadScene("TitleTestScene");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("�� ���� �Ϸ�");
            SceneManager.LoadScene("RoomTestScene");
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.CurrentRoom.IsVisible = true;
            if (!lobbyBGM.isPlaying)
                lobbyBGM.Play();

            if (PhotonNetwork.IsMasterClient)
            {
                // master�� ready�� ���·� �濡 ����
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
            Debug.Log("�� ������ �Ϸ�");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("�÷��̾� ����");
            RoomPanel roomPanel = GameObject.Find("Canvas").GetComponent<RoomPanel>();
            roomPanel?.UpdateRoomState();
            roomPanel?.UpdateLocalPlayerPropertiesUpdate();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("�÷��̾� ����");
            RoomPanel roomPanel = GameObject.Find("Canvas").GetComponent<RoomPanel>();
            roomPanel?.UpdateRoomState();
            roomPanel?.UpdateLocalPlayerPropertiesUpdate();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            Debug.Log("���� ����");
            RoomPanel roomPanel = GameObject.Find("Canvas").GetComponent<RoomPanel>();
            roomPanel?.UpdateRoomState();
            roomPanel?.UpdateLocalPlayerPropertiesUpdate();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            Debug.Log("�÷��̾� ���� ��ȭ");

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
            Debug.Log("�κ� ���� �Ϸ�");
            if(!lobbyBGM.isPlaying)
                lobbyBGM.Play();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("�� ���� ����");
            LobbyPanel lobbyPanel = GameObject.Find("Canvas").transform.GetChild(3).GetComponent<LobbyPanel>();
            lobbyPanel?.UpdateRoomList(roomList);
        }

        public override void OnLeftLobby()
        {
            Debug.Log("�κ� ����");
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log(string.Format("�� ���� ���� {0} : {1}", returnCode, message));
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log(string.Format("�� ���� ���� {0} : {1}", returnCode, message));
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log(string.Format("���� �� ���� ���� {0} : {1}", returnCode, message));
            Debug.Log("���� �����ؼ� �����մϴ�.");

            // TODO : �� �̸� ���� ����
            string roomName = string.Format("Room {0}", Random.Range(1000, 10000));
            RoomOptions options = new RoomOptions() { MaxPlayers = (byte)12 };
            PhotonNetwork.CreateRoom(roomName, options);

            SceneManager.LoadScene("RoomTestScene");
        }
    }
}
