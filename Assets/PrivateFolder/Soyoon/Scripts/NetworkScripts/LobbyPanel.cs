using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoYoon
{
    public class LobbyPanel : MonoBehaviour
    {
        [SerializeField]
        private RoomEntry roomEntryPrefab;
        [SerializeField]
        private RectTransform roomContent;

        private List<RoomEntry> roomEntries;

        private void Awake()
        {
            roomEntries = new List<RoomEntry>();
        }

        private void Start()
        {
            PhotonNetwork.JoinLobby();
        }

        public void UpdateRoomList(List<RoomInfo> roomList)
        {
            foreach (RoomEntry room in roomEntries)
            {
                Destroy(room.gameObject);
            }
            roomEntries.Clear();

            foreach (RoomInfo room in roomList)
            {
                RoomEntry entry = Instantiate(roomEntryPrefab, roomContent);
                entry.Initialized(room.Name, room.PlayerCount, room.MaxPlayers);
                roomEntries.Add(entry);
            }
        }
    }
}
