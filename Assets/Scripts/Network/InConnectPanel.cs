using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class InConnectPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject createRoomPanel;

    [SerializeField]
    private TMP_InputField roomNameInputField;
   


   private void OnEnable()
   {
       createRoomPanel.SetActive(false);
   }

    public void OnCreateRoomButtonClicked()
    {
        createRoomPanel.SetActive(true);
    }

    public void OnCreateRoomCancelButtonClicked()
    {
        createRoomPanel.SetActive(false);
    }

    public void OnCreateRoomConfirmButtonClicked()
    {
        string roomName = roomNameInputField.text;
        if (roomName == "")
            roomName = string.Format("Room{0}", Random.Range(1000, 10000));

        int maxPlayer = 12;

        RoomOptions options = new RoomOptions { MaxPlayers = (byte)maxPlayer };
        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public void OnRandomMatchingButtonClicked()
    {
        PhotonNetwork.JoinRandomRoom();
    }


    //-------------------------------------------------

    public void OnLobbyButtonClicked()
    {
        PhotonNetwork.JoinLobby();
    }

    public void OnLogoutButtonClicked()
    {
        PhotonNetwork.Disconnect();
    }
}