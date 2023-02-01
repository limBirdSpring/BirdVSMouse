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


    //ä��â Ŭ������ ��
    public void OnChatPanelClicked()
    {
        bigChatObject.SetActive(true);
    }


    //ä��â �ܺθ� Ŭ��������
    public void OnChatPanelExitClicked()
    {
        bigChatObject.SetActive(false);
    }

    //ä�� ������ Ŭ������ ��
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
        if (inputField.text != "" && Input.GetKeyDown(KeyCode.F1))//���� ��ư�� ������ (�����ʿ�)
        {
            string message = userName + " : " + inputField.text;

            AddChat(message);

            photonView.RPC("RPC_Chat", RpcTarget.All, message);
            inputField.text = "";
        }
    }

    //�ٸ�������� �� ä�õ� ���
    private void AddChat(string message)
    {
        GameObject text;

        //�ؽ�Ʈ�����տ� �޽����ְ� ��ġ ����

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
