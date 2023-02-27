using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

        private AudioSource lobbyBGM;
        private void Awake()
        {
            playerEntries = new List<PlayerEntry>();
            lobbyBGM = GameObject.FindObjectOfType<LobbyManager>().GetComponent<AudioSource>();
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
            Debug.Log("플레이어 갱신");

            Initialize();
            
            // 각자의 entry를 destroy 한 뒤 생성
            foreach (PlayerEntry player in playerEntries)
            {
                Destroy(player.gameObject);
            }
            playerEntries.Clear();

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                // 플레이어들 다시 추가
                PlayerEntry entry = Instantiate(playerEntryPrefab, playerContent);
                object charNum;
                if (!player.CustomProperties.TryGetValue("CharNum", out charNum))
                    charNum = -1;
                object badge1Num;
                if (!player.CustomProperties.TryGetValue("Badge1Num", out badge1Num))
                    badge1Num = -1;
                object badge2Num;
                if (!player.CustomProperties.TryGetValue("Badge2Num", out badge2Num))
                    badge2Num = -1;

                // TODO : 뱃지도 같은 형식으로 추가

                entry.Initialize(player.ActorNumber, player.NickName, (string)charNum, (string)badge1Num, (string)badge2Num, player.IsMasterClient);
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
            if (PhotonNetwork.PlayerList.Length % 2 != 0)
                return;
//            if (PhotonNetwork.PlayerList.Length < 8)
//                return;

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
            if (lobbyBGM.isPlaying)
                lobbyBGM.Stop();

            PhotonNetwork.LoadLevel("GameScene");
        }

        public void OnLeaveRoomButtonClicked()
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}