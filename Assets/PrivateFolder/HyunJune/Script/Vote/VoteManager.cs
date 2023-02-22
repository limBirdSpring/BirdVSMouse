using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Saebom;
using SoYoon;
using Photon.Pun.Demo.PunBasics;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using HyunJune;
using TMPro;
using static UnityEngine.Rendering.DebugUI;


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

    public List<int> voteCompletePlayerList = new List<int>();

    [SerializeField]
    private SkipVote skipVote;
    [SerializeField]
    private TMP_Text timer;

    [SerializeField]
    private PlayerControllerTest[] controllers;

    [Header("VoteChatting")]
    [SerializeField]
    private TextBox myTextBoxPrefab;
    [SerializeField]
    private TextBox otherTextBoxPrefab;
    [SerializeField]
    private TMP_InputField chatInputField;
    [SerializeField]
    private Transform chatContent;

    private List<TextBox> textList = new List<TextBox>();

    [Header("VoteToDeathWindow")]
    [SerializeField]
    private VoteToDeath voteToDeathWindow;

    public bool deadBodyFinder = false;
    private bool voteComplete = false;
    private int participantCount;
    private VoteRole myRole;

    [Header("SkipWindow")]
    [SerializeField]
    private ResultNothing skipWindow;

    private IEnumerator co;

    private void Awake()
    {
        Instance = this;
        //FindObjectsOfType<PlayerController>();
        chatInputField.characterLimit = 50;
        controllers = FindObjectsOfType<PlayerControllerTest>();
    }

    // ������ �Ѵ�
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            //FindDeadBody();
            photonView.RPC("EmergencyReport", RpcTarget.All, null);
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            //FindDeadBody();
            photonView.RPC("PlayerKill", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        }

        if (Input.GetButtonDown("Submit"))
        {
            if (chatInputField.IsActive() && chatInputField.text != "")
            {
                photonView.RPC("SendMessage", RpcTarget.All, chatInputField.text, PhotonNetwork.LocalPlayer.ActorNumber);
                chatInputField.text = "";
                chatInputField.ActivateInputField();
            }
            else
            {
                chatInputField.ActivateInputField();
            }               
        }
    }

    public void OnPressedSendButton()
    {
        if (chatInputField.text == "")
            return;

        photonView.RPC("SendMessage", RpcTarget.All, chatInputField.text, PhotonNetwork.LocalPlayer.ActorNumber);
        chatInputField.text = "";
    }

    private IEnumerator StartTimer()
    {
        float time = 99;
        while (time > 0)
        {
            time -= Time.deltaTime;
            Debug.Log(time);
            photonView.RPC("UpdateTime", RpcTarget.All, time);
            yield return null;
        }

        Debug.Log("Ÿ�ӿ���");
        time = 0;
        this.timer.text = time.ToString("F0") + " s";
        photonView.RPC("FocedSkip", RpcTarget.All, null);
    }

    [PunRPC]
    public void UpdateTime(float time)
    {
        timer.text = time.ToString("F0") + " s";
    }

    [PunRPC]
    public void FocedSkip()
    {
        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            // �����ڸ� ����
            if (spectatorList.Contains(player.Value.ActorNumber))
                return;

            // ����ڸ� ����
            if (deadList.Contains(player.Value.ActorNumber))
                return;

            // ��ǥ�� ������ ����
            if (voteCompletePlayerList.Contains(player.Value.ActorNumber))
                return;


            // ���� ��ŵ �ߵ�
            VoteSkip();
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
                SoundManager.Instance.PlayUISound(UISFXName.Chat);
                textList.Add(text);
                return;
            }  
        }

        // ä���� ���� ����� ���� �ƴ϶�� ���� ���ҿ� ���� ä���� �к�
        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (actorNumeber != player.Value.ActorNumber)
                continue;

            switch (myRole)
            {
                // ���� �������� ���
                case VoteRole.Participant:
                    // ä�� ���� �÷��̾ �������� ��츸 �޴´�
                    if (participantList.Contains(actorNumeber) && !deadList.Contains(actorNumeber))
                    {
                        TextBox text = Instantiate(otherTextBoxPrefab, chatContent);
                        text.SetMessage(player.Value, message);
                        SoundManager.Instance.PlayUISound(UISFXName.Chat);
                        textList.Add(text);
                    }
                    break;
                // ���� �������� ��� �������� ä��
                case VoteRole.Spectator:
                    // ä�� ���� �÷��̾ ����� �� ��� �ȹ޴´�
                    if (deadList.Contains(actorNumeber))
                        return;
                             
                    TextBox text2 = Instantiate(otherTextBoxPrefab, chatContent);
                    text2.SetMessage(player.Value, message);
                    SoundManager.Instance.PlayUISound(UISFXName.Chat);
                    textList.Add(text2);
                    break;
                // ���� ������� ��� ��� ä���� �޴´�
                case VoteRole.Dead:
                    TextBox text3 = Instantiate(otherTextBoxPrefab, chatContent);
                    text3.SetMessage(player.Value, message);
                    SoundManager.Instance.PlayUISound(UISFXName.Chat);
                    textList.Add(text3);
                    break;
            }
        }
    }

    public void FindDeadBody()
    {
        // ��ü �߰� �ؼ� RPC�� ��ο��� EmergencyReport �Ѵ�
        if ((PlayGameManager.Instance.myPlayerState.isBird && !TimeManager.Instance.isCurNight) ||
           (!PlayGameManager.Instance.myPlayerState.isBird && TimeManager.Instance.isCurNight))
        {
            deadBodyFinder = true;
            photonView.RPC("EmergencyReport", RpcTarget.All, null);
            MissionButton.Instance.Emergency();
        }
    }


    [PunRPC]
    public void EmergencyReport()
    {
        // ��� ����
        SetUpPlayerState();
        AddAlivePlayerEntry();
        SetRole();
        skipVote.Initialized();
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            co = StartTimer();
            StartCoroutine(co);
            TimeManager.Instance.TimeStop();
        }

        voteWindow.gameObject.SetActive(true);
        SoundManager.Instance.PlayUISound(UISFXName.Vote);
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
            Destroy(playerVoteEntries[i].gameObject);
        }
        playerVoteEntries.Clear();

        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            // �ش� �÷��̾ �����ڸ� ���� x
            if (spectatorList.Contains(player.Key))
                continue;

            PlayerVoteEntry entry = Instantiate(voteEntryPrefab, entryContent);
            entry.Initialized(player.Value);
            //if (deadList.Contains(player.Key))
            //{
            //    entry.DeadSetting();
            //}
            playerVoteEntries.Add(entry);
        }
    }

    public void VoteSkip()
    {
        if (voteComplete)
            return;

        if (spectatorList.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
            return;

        if (deadList.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
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

        VotingResult();
    }

    public void Vote(int target)
    {
        // �̹� ��ǥ�� ������ ����
        if (voteComplete)
            return;

        if (spectatorList.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
            return;

        if (deadList.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
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

        VotingResult();
    }

    public int CalculateAlivePlayer()
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

        participantCount = alivePlayerCount;

        return participantCount;
    }

    public void VotingResult()
    {
        // ������ �ƴ϶�� ���⼭ ����
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            return;

        CalculateAlivePlayer();

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
            photonView.RPC("NothingWindowOpen", RpcTarget.All, null);
        }
    }

    [PunRPC]
    private void NothingWindowOpen()
    {
        skipWindow.gameObject.SetActive(true);
        StartCoroutine(VotingEnd());
    }

    [PunRPC]
    private void PlayerKill(int actorNumber)
    {
        // ���� ����� ����
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            // ��Ʈ�ѷ��� ��� �ҷ����ְ�
            controllers = FindObjectsOfType<PlayerControllerTest>();
            // ��Ʈ�ѷ��� for������ ������
            for (int i = 0; i < controllers.Length; i++)
            {
                // ���� ����� ���ͳѹ��� ������ ���ͳѹ��� ���� �ִ� ��Ʈ�ѷ��� ã�� ����
                if (controllers[i].photonView.OwnerActorNr == actorNumber)
                {
                    // ���δ�
                    controllers[i].photonView.RPC("VoteDie", RpcTarget.All);
                }
                else
                    continue;
            }
        }
        
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
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            StopCoroutine(co);
            TimeManager.Instance.TimeResume();
            CheckGameOver();
        }

        ResetText();
        voteWindow.SetActive(false);
        voteToDeathWindow.Init();
        deadBodyFinder = false;
        voteComplete = false;
        myRole = VoteRole.None;
        participantCount = 0;
        voteToDeathWindow.gameObject.SetActive(false);
        voteCompletePlayerList.Clear();
        skipWindow.gameObject.SetActive(false);
    }

    private void ResetText()
    {
        for(int i = 0; i < textList.Count; i++)
        {
            Destroy(textList[i].gameObject);
        }
        textList.Clear();
    }

    public void CheckGameOver()
    {
        int spy = 0;
        int noneSpy = 0;

        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            // ������ ����
            if (spectatorList.Contains(player.Value.ActorNumber))
                continue;

            // ����� ����
            if (PlayGameManager.Instance.playerList[player.Value.GetPlayerNumber()].isDie)
                continue;

            // �����̸� ������ ����
            if (PlayGameManager.Instance.playerList[player.Value.GetPlayerNumber()].isSpy)
                spy++;
            // �Ϲ� �ù��̸� �ù� ����
            else
                noneSpy++;
        }

        //�����̰� ������ �ù� �¸�
        if (spy == 0)
        {
            photonView.RPC("GameOver", RpcTarget.All, null);
        }
        // �����̿� �ù��� ���� �ο��̸� �������� �¸�
        else if (spy == noneSpy)
        {
            photonView.RPC("GameOver", RpcTarget.All, null);
        }
        // �Ѵ� �ƴϸ� ���� ����
        else
            return;
    }

    [PunRPC]
    private void GameOver()
    {
        if (PhotonNetwork.IsMasterClient)
            ScoreManager.Instance.ActiveTimeOverNow();
    }


    public void ToggleAllButton(bool toggle)
    {
        skipVote.ToggleButton(toggle); 
        foreach (PlayerVoteEntry entry in playerVoteEntries)
        {
            entry.ToggleButton(toggle);
        }
    }
}
