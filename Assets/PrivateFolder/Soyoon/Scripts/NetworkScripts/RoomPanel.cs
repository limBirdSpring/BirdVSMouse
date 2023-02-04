using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace SoYoon
{
    public class RoomPanel : MonoBehaviour
    {
        [SerializeField]
        private PlayerEntry playerEntryPrefab;
        [SerializeField]
        private RectTransform playerContent;
        [SerializeField]
        private Button startButton;
        [SerializeField]
        private Button readyButton;

        private List<PlayerEntry> playerEntries;

        private void Awake()
        {
            playerEntries = new List<PlayerEntry>();
        }

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                readyButton.gameObject.SetActive(false);
                startButton.gameObject.SetActive(true);
                startButton.interactable = false;
            }
            else
            {
                readyButton.gameObject.SetActive(true);
                startButton.gameObject.SetActive(false);
            }
        }

        public void OnReadyButtonClicked()
        {
            foreach (PlayerEntry player in playerEntries)
            {
                if (player.OwnerId == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    object isPlayerReady;
                    if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Ready", out isPlayerReady))
                        isPlayerReady = false;

                    bool ready = (bool)isPlayerReady;

                    player.SetPlayerReady(!ready);

                    Hashtable props = new Hashtable();
                    props.Add("Ready", !ready);

                    PhotonNetwork.LocalPlayer.SetCustomProperties(props);
                    return;
                }
            }
        }

        public void UpdateRoomState()
        {
            Debug.Log("�÷��̾� ����");

            Initialize();
            
            // ������ entry�� destroy �� �� ����
            foreach (PlayerEntry player in playerEntries)
            {
                Destroy(player.gameObject);
            }
            playerEntries.Clear();

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                //// �߰� �� ����� ������ ó��
                //object isKicked;
                //if (!player.CustomProperties.TryGetValue("IsKicked", out isKicked))
                //    isKicked = false;
                //if((bool)isKicked && player == PhotonNetwork.LocalPlayer)
                //{
                //    PhotonNetwork.LeaveRoom();
                //    Hashtable props = new Hashtable();
                //    props.Add("IsKicked", false);
                //    player.SetCustomProperties(props);
                //}    

                // �÷��̾�� �ٽ� �߰�
                PlayerEntry entry = Instantiate(playerEntryPrefab, playerContent);
                object charNum;
                if (!player.CustomProperties.TryGetValue("CharNum", out charNum))
                    charNum = -1;

                // TODO : ������ ���� �������� �߰�

                entry.Initialize(player.ActorNumber, player.NickName, (int)charNum, 0, 0, player.IsMasterClient);
                object isPlayerReady;
                if (player.CustomProperties.TryGetValue("Ready", out isPlayerReady))
                {
                    entry.SetPlayerReady((bool)isPlayerReady);
                }
                playerEntries.Add(entry);
            }
        }

        public void UpdateLocalPlayerPropertiesUpdate()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            //if (!(PhotonNetwork.PlayerList.Length != 12))
            //    return;

            if (CheckPlayerReady())
                startButton.interactable = true;
            else
                startButton.interactable = false;
        }

        public bool CheckPlayerReady()
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                object isPlayerReady;
                if (player.CustomProperties.TryGetValue("Ready", out isPlayerReady))
                {
                    if (!(bool)isPlayerReady)
                        return false;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public void OnStartButtonClicked()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel("GameScene");
        }

        public void OnLeaveRoomButtonClicked()
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}