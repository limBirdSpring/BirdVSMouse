using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Saebom;
using Photon.Pun.Demo.PunBasics;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

public class VoteManager : MonoBehaviourPun
{
    public static VoteManager Instance;

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

    public bool deadBodyFinder = false;
    private bool voteComplete = false;

    private void Awake()
    {
        Instance = this;
        //FindObjectsOfType<PlayerController>();
    }

    // ������ �Ѵ�
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            FindDeadBody();
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

        voteWindow.gameObject.SetActive(true);
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

        // ��� ��?
    }

    [PunRPC]
    private void VotingEnd()
    {
        // ��ǥ ����
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
