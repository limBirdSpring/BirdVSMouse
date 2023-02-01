using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SoYoon
{
    public class MakeRoomUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField roomNameInputField;
        public void OnCreateRoomCancelButtonClicked()
        {
            this.gameObject.SetActive(false);
            roomNameInputField.text = "";
        }

        public void OnCreateRoomConfirmButtonClicked()
        {
            string roomName = roomNameInputField.text;
            if (roomName == "")
                roomName = string.Format("Room {0}", Random.Range(1000, 10000));
            // TODO : 랜덤 방명 생성

            int fixedPlayerNum = 12;

            RoomOptions options = new RoomOptions() { MaxPlayers = (byte)fixedPlayerNum };
            PhotonNetwork.CreateRoom(roomName, options);
            this.gameObject.SetActive(false);
        }
    }
}
