using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class ChatManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject chatPanel;

    [SerializeField]
    private GameObject bigChatObject;

    [SerializeField]
    private GameObject miniChatPanel;

    [SerializeField]
    private TMP_InputField inputField;

    PhotonView photonView;

    [SerializeField]
    private GameObject textPrefab;

    private string userName;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        
    }


    //채팅창 클릭했을 떄
    public void OnChatPanelClicked()
    {
        bigChatObject.SetActive(true);
    }


    //채팅창 외부를 클릭했을때
    public void OnChatPanelExitClicked()
    {
        bigChatObject.SetActive(false);
    }

    //채팅 공간을 클릭했을 떄
    public void OnChatSpaceClicked()
    {
        inputField.ActivateInputField();
    }

    private void Update()
    {
        if (chatPanel.activeSelf == true)
           SendMessage();
    }

    private void SendMessage()
    {
        if (inputField.text != "" && Input.GetKeyDown(KeyCode.F1))//엔터 버튼을 누르면 (수정필요)
        {
            string message = userName + " : " + inputField.text;

            AddChat(message);

            photonView.RPC("RPC_Chat", RpcTarget.All, message);
            inputField.text = "";
        }
    }

    //다른사람에게 온 채팅도 띄움
    private void AddChat(string message)
    {
        GameObject text;

        //텍스트프리팹에 메시지넣고 위치 조정

        text = Instantiate(textPrefab, chatPanel.transform);
        text.GetComponent<TextMeshProUGUI>().text = message;
        //chatPanel.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
   
        text = Instantiate(textPrefab, miniChatPanel.transform);
        text.GetComponent<TextMeshProUGUI>().text = message;
        //miniChatPanel.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

    }

    [PunRPC]
    void RPC_Chat(string message)
    {
        AddChat(message);
    }
}
