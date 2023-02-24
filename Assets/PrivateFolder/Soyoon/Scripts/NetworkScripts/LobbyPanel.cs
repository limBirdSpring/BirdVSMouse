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

        private Dictionary<string, RoomEntry> allRoomEntries;

        private void Awake()
        {
            allRoomEntries = new Dictionary<string, RoomEntry>();
        }

        private void Start()
        {
            PhotonNetwork.JoinLobby();
        }

        public void UpdateRoomList(List<RoomInfo> roomList)
        {
            foreach(RoomInfo room in roomList)
            {
                if(allRoomEntries.ContainsKey(room.Name))
                {
                    if (room.PlayerCount == 0)
                    {
                        Destroy(allRoomEntries[room.Name].gameObject);
                        allRoomEntries.Remove(room.Name);
                    }
                    else
                    {
                        RoomEntry entry = allRoomEntries[room.Name];
                        entry.Initialized(room.Name, room.PlayerCount, room.MaxPlayers);
                        allRoomEntries[room.Name] = entry;
                    }
                }
                else
                {
                    if (room.PlayerCount == 0)
                        return;
                    RoomEntry entry = Instantiate(roomEntryPrefab, roomContent);
                    entry.Initialized(room.Name, room.PlayerCount, room.MaxPlayers);
                    allRoomEntries.Add(room.Name, entry);
                }
            }
        }
    }
}
