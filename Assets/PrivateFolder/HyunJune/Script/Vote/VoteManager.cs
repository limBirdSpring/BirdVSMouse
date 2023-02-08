using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Saebom;
using Photon.Pun.Demo.PunBasics;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using HyunJune;
using TMPro;


public enum VoteRole
{
    None,
    Participant,
    Spectator,
    Dead
}

public class VoteManager : MonoBehaviourPun
{
    public static VoteManager Instance;

    [Header("Vote")]
    [SerializeField]
    private GameObject voteWindow;

    public List<int> participantList = new List<int>();
    public List<int> spectatorList = new List<int>();
    public List<int> deadList = new List<int>();

    [SerializeField]
    private PlayerVoteEntry voteEntryPrefab;
    [SerializeField]
    private Transform entryContent;
    [SerializeField]
    private List<PlayerVoteEntry> playerVoteEntries = new List<PlayerVoteEntry>();

    private List<int> voteCompletePlayerList = new List<int>();

    [SerializeField]
    private Button skipButton;
    [SerializeField]
    private SkipVote skipVote;

    [SerializeField]
    private PlayerController controller;

    [Header("VoteChatting")]
    [SerializeField]
    private TextBox myTextBoxPrefab;
    [SerializeField]
    private TextBox otherTextBoxPrefab;
    [SerializeField]
    private TMP_InputField chatInputField;
    [SerializeField]
    private Transform chatContent;

    [Header("VoteToDeathWindow")]
    [SerializeField]
    private VoteToDeath voteToDeathWindow;

    public bool deadBodyFinder = false;
    private bool voteComplete = false;
    private VoteRole myRole;

    private void Awake()
    {
        Instance = this;
        //FindObjectsOfType<PlayerController>();
        chatInputField.characterLimit = 30;
    }

    // ������ �Ѵ�
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            FindDeadBody();
        }       

        if (Input.GetButtonDown("Submit"))
        {
            if (chatInputField.IsActive() && chatInputField.text != "")
            {
                photonView.RPC("SendMessage", RpcTarget.All, chatInputField.text, PhotonNetwork.LocalPlayer.ActorNumber);
            }
            else
            {
                chatInputField.ActivateInputField();
            }
                
        }
    }


    [PunRPC]
    private void SendMessage(string message, int actorNumeber)
    {

        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            // ä���� ������ ����� ����� �� ���������� �ٷ� ��ȭ ����
            if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumeber &&
                PhotonNetwork.LocalPlayer.ActorNumber == player.Value.ActorNumber)
            {
                TextBox text = Instantiate(myTextBoxPrefab, chatContent);
                text.SetMessage(player.Value, message);
                return;
            }  
        }

        // ä���� ���� ����� ���� �ƴ϶�� ���� ���ҿ� ���� ä���� �к�
        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber != player.Value.ActorNumber)
                return;

            switch (myRole)
            {
                // ���� �������� ���
                case VoteRole.Participant:
                    // ä�� ���� �÷��̾ �������� ��츸 �޴´�
                    if (participantList.Contains(actorNumeber))
                    {
                        TextBox text = Instantiate(otherTextBoxPrefab, chatContent);
                        text.SetMessage(player.Value, message);
                    }
                    break;
                // ���� �������� ��� �������� ä��
                case VoteRole.Spectator:
                    // ä�� ���� �÷��̾ ����� �� ��� �ȹ޴´�
                    if (deadList.Contains(actorNumeber))
                        return;
                             
                    TextBox text2 = Instantiate(otherTextBoxPrefab, chatContent);
                    text2.SetMessage(player.Value, message);
                    break;
                // ���� ������� ��� ��� ä���� �޴´�
                case VoteRole.Dead:
                    TextBox text3 = Instantiate(otherTextBoxPrefab, chatContent);
                    text3.SetMessage(player.Value, message);
                    break;
            }
        }
    }

    public void FindDeadBody()
    {
        // ��ü �߰� �ؼ� RPC�� ��ο��� EmergencyReport �Ѵ�
        deadBodyFinder = true;
        photonView.RPC("EmergencyReport", RpcTarget.All, null);
    }


    [PunRPC]
    public void EmergencyReport()
    {
        // ��� ����
        SetUpPlayerState();
        AddAlivePlayerEntry();
        SetRole();

        voteWindow.gameObject.SetActive(true);
    }

    public void SetRole()
    {
        if (participantList.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
            myRole = VoteRole.Participant;
        else if (spectatorList.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
            myRole = VoteRole.Spectator;


        if (deadList.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
            myRole = VoteRole.Dead;
    }

    public void SetUpPlayerState()
    {
        participantList.Clear();
        spectatorList.Clear();
        deadList.Clear();

        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (PlayGameManager.Instance.playerList[player.Value.GetPlayerNumber()].isDie)
            {
                // �׾����� ����ڿ� �ִ´�
                deadList.Add(player.Key);
            }
            
            if (TimeManager.Instance.isCurNight) // ��
            {
                if (!PlayGameManager.Instance.playerList[player.Value.GetPlayerNumber()].isBird)
                { 
                    // �㸦 �����ڿ� �ִ´�
                    participantList.Add(player.Key);
                }
                else
                {
                    // ���� �����ڿ� �ִ´�
                    spectatorList.Add(player.Key);
                }
            }
            else // ��
            {
                if (PlayGameManager.Instance.playerList[player.Value.GetPlayerNumber()].isBird)
                {
                    // ���� �����ڿ� �ִ´�
                    participantList.Add(player.Key);
                }
                else
                {
                    // �㸦 �����ڿ� �ִ´�
                    spectatorList.Add(player.Key);
                }
            }
        }
    }

    private void AddAlivePlayerEntry() // ������ �÷��̾� ��Ʈ�� �߰�
    {
        // playerVoteEntries �ʱ�ȭ
        for (int i = 0; i < playerVoteEntries.Count; i++)
        {
            Destroy(playerVoteEntries[0].gameObject);
        }

        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            // �ش� �÷��̾ �����ڸ� ���� x
            if (spectatorList.Contains(player.Key))
                continue;

            PlayerVoteEntry entry = Instantiate(voteEntryPrefab, entryContent);
            entry.Initialized(player.Value);
            if (deadList.Contains(player.Key))
            {
                entry.DeadSetting();
            }
            playerVoteEntries.Add(entry);
        }
    }

    public void VoteSkip()
    {
        if (voteComplete)
            return;

        voteComplete = true;
        ToggleAllButton(false);
        photonView.RPC("SetSkipCount", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
    }

    [PunRPC]
    public void SetSkipCount(int actorNumber)
    {
        skipVote.AddSkipVoteCount();

        foreach (PlayerVoteEntry entry in playerVoteEntries)
        {
            // ���� ��ǥ�� ����� ���ͳѹ��� ��Ʈ���� ���ͳѹ��� �Ȱ��ٸ� ��ǥ�Ϸ� ������Ʈ
            if (entry.ActorNumber == actorNumber)
            {
                entry.CompleteVote();
                voteCompletePlayerList.Add(entry.ActorNumber);
            }
        }
    }

    public void Vote(int target)
    {
        // �̹� ��ǥ�� ������ ����
        if (voteComplete)
            return;

        // �� �ڽ��̸� ����
        if (PhotonNetwork.LocalPlayer.ActorNumber == target)
            return;

        voteComplete = true;
        ToggleAllButton(false);
        photonView.RPC("VoteCheckRPC", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, target);
    }

    [PunRPC]
    public void VoteCheckRPC(int actorNumber, int target)
    {
        int alivePlayerCount = 0;
        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            // �����ڸ� ��Ƽ��
            if (spectatorList.Contains(player.Key))
                continue;

            // �׾����� ��Ƽ��
            if (PlayGameManager.Instance.playerList[player.Value.GetPlayerNumber()].isDie)
                continue;

            alivePlayerCount++;
        }

        int participantCount = alivePlayerCount;

        foreach (PlayerVoteEntry entry in playerVoteEntries)
        {
            // ���� ��ǥ�� ����� ���ͳѹ��� ��Ʈ���� ���ͳѹ��� �Ȱ��ٸ� ��ǥ�Ϸ� ������Ʈ
            if (entry.ActorNumber == actorNumber)
            {
                entry.CompleteVote();
            }

            // Ÿ���� ���ͳѹ��� ��Ʈ���� ���ͳѹ��� �Ȱ��ٸ� ��ǥ �� ���
            if (entry.ActorNumber == target)
            {
                entry.AddVoteCount();
            }
        }


        // ��ǥ �Ϸ��ڵ��� ��Ͽ� ���� ������ �߰�
        if (!voteCompletePlayerList.Contains(actorNumber))
        {
            voteCompletePlayerList.Add(actorNumber);
        }


        // ������ �ƴ϶�� ���⼭ ����
        if (!PhotonNetwork.IsMasterClient)
            return;

        // ��ǥ�� ���� �������� ������ ������ ����
        if (voteCompletePlayerList.Count < participantCount)
            return;


        int mostVoterCount = 0;     // ��ǥ��
        int mostVoterPlayer = -1;   // �ִ� ��ǥ��

        foreach (PlayerVoteEntry entry in playerVoteEntries)
        {
            if (entry.VoteCount > mostVoterCount)
            {
                mostVoterCount = entry.VoteCount;
                mostVoterPlayer = entry.ActorNumber;
            }
        }

        // �ִ� ��ǥ ���� �������� ���� �̻��� ǥ�� �޾����� �׿�������
        if (mostVoterCount >= participantCount / 2)
        {
            // ���δ�
            photonView.RPC("PlayerKill", RpcTarget.All, mostVoterPlayer);
        }
        else
        {
            // �Ƹ� ��ŵ
            photonView.RPC("VotingEnd", RpcTarget.All, null);
        }
    }

    [PunRPC]
    private void PlayerKill(int actorNumber)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
            controller.Die();
        
        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.ActorNumber != actorNumber)
                continue;

            voteToDeathWindow.Init();
            voteToDeathWindow.Setting(PlayGameManager.Instance.playerList[player.Value.GetPlayerNumber()]);
        }


        voteToDeathWindow.gameObject.SetActive(true);
        StartCoroutine(VotingEnd());
    }

    public IEnumerator VotingEnd()
    {
        yield return new WaitForSeconds(10);
        photonView.RPC("VotingEndRPC", RpcTarget.All, null);
    }

    [PunRPC]
    private void VotingEndRPC()
    {
        // ��ǥ ����
        voteToDeathWindow.Init();
        deadBodyFinder = false;
        voteComplete = false;
        voteWindow.SetActive(false);
    }

    public void ToggleAllButton(bool toggle)
    {
        skipButton.interactable = toggle;
        foreach (PlayerVoteEntry entry in playerVoteEntries)
        {
            entry.ToggleButton(toggle);
        }
    }
}
