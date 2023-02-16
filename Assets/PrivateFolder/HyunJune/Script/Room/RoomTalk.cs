using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using HyunJune;
using UnityEditor;

public class RoomTalk : MonoBehaviourPun
{
    [SerializeField]
    private GameObject miniChat;
    [SerializeField]
    private GameObject bigChat;
    [SerializeField]
    private Transform miniContent;
    [SerializeField]
    private Transform bigContent;
    [SerializeField]
    private TMP_Text textPrefab;
    [SerializeField]
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField.characterLimit = 50;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (inputField.IsActive() && inputField.text != "")
            {
                photonView.RPC("SendMessage", RpcTarget.All, inputField.text, PhotonNetwork.LocalPlayer.ActorNumber);
                inputField.text = "";
                inputField.ActivateInputField();
            }
            else
            {
                miniChat.SetActive(false);
                bigChat.SetActive(true);
                inputField.ActivateInputField();
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            inputField.DeactivateInputField();
            ChatOff();
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            int id = Random.Range(1, 10000);
            PhotonNetwork.LocalPlayer.NickName = id.ToString();
        }
    }

    public void OnPressedSendButton()
    {
        if (inputField.text == "")
            return;

        photonView.RPC("SendMessage", RpcTarget.All, inputField.text, PhotonNetwork.LocalPlayer.ActorNumber);
        inputField.text = "";
    }

    [PunRPC]
    private void SendMessage(string message, int actorNumeber)
    {
        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            // 채팅을 보내는 사람이 나라면 내 프리팹으로 바로 대화 생성
            if (player.Value.ActorNumber == actorNumeber)
            {
                TMP_Text miniMessage = Instantiate(textPrefab, miniContent);
                miniMessage.text = string.Format("{0} : " + message, player.Value.NickName); ;
                TMP_Text bigMessage = Instantiate(textPrefab, bigContent);
                bigMessage.text = string.Format("{0} : " + message, player.Value.NickName); ;
                //SoundManager.Instance.PlayUISound(UISFXName.Chat);
                return;
            }
        }
    }

    public void ChatOff()
    {
        miniChat.SetActive(true);
        bigChat.SetActive(false);
    }
}
