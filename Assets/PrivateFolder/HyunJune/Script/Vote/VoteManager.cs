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
using static System.Net.Mime.MediaTypeNames;


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
    private SkipVote skipVote;
    [SerializeField]
    private TMP_Text timer;

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

    private List<TextBox> textList = new List<TextBox>();

    [Header("VoteToDeathWindow")]
    [SerializeField]
    private VoteToDeath voteToDeathWindow;

    public bool deadBodyFinder = false;
    private bool voteComplete = false;
    private int participantCount;
    private VoteRole myRole;

    private void Awake()
    {
        Instance = this;
        //FindObjectsOfType<PlayerController>();
        chatInputField.characterLimit = 30;
    }

    private void OnEnable()
    {
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(StartTimer(99f));
    }

    // 지워야 한다
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
                chatInputField.text = "";
            }
            else
            {
                chatInputField.ActivateInputField();
            }
                
        }
    }

    private IEnumerator StartTimer(float time)
    {
        while (time < 0)
        {
            time -= Time.deltaTime;
            timer.text = time.ToString("F0");
            yield return null;
        }

        time = 0;
        timer.text = time.ToString("F0");
        photonView.RPC("FocedSkip", RpcTarget.All, null);
    }

    [PunRPC]
    public void FocedSkip()
    {
        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            // 참가자면 리턴
            if (spectatorList.Contains(player.Value.ActorNumber))
                return;

            // 사망자면 리턴
            if (deadList.Contains(player.Value.ActorNumber))
                return;

            // 투표를 했으면 리턴
            if (voteCompletePlayerList.Contains(player.Value.ActorNumber))
                return;

            // 강제 스킵 발동
            VoteSkip();
        }
    }

    [PunRPC]
    private void SendMessage(string message, int actorNumeber)
    {

        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            // 채팅을 보내는 사람이 나라면 내 프리팹으로 바로 대화 생성
            if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumeber &&
                PhotonNetwork.LocalPlayer.ActorNumber == player.Value.ActorNumber)
            {
                TextBox text = Instantiate(myTextBoxPrefab, chatContent);
                text.SetMessage(player.Value, message);
                textList.Add(text);
                return;
            }  
        }

        // 채팅을 보낸 사람이 내가 아니라면 나의 역할에 따라 채팅을 분별
        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber != player.Value.ActorNumber)
                return;

            switch (myRole)
            {
                // 내가 참가자일 경우
                case VoteRole.Participant:
                    // 채팅 보낸 플레이어가 참가자일 경우만 받는다
                    if (participantList.Contains(actorNumeber))
                    {
                        TextBox text = Instantiate(otherTextBoxPrefab, chatContent);
                        text.SetMessage(player.Value, message);
                        textList.Add(text);
                    }
                    break;
                // 내가 관전자일 경우 참가자의 채팅
                case VoteRole.Spectator:
                    // 채팅 보낸 플레이어가 사망자 일 경우 안받는다
                    if (deadList.Contains(actorNumeber))
                        return;
                             
                    TextBox text2 = Instantiate(otherTextBoxPrefab, chatContent);
                    text2.SetMessage(player.Value, message);
                    textList.Add(text2);
                    break;
                // 내가 사망자일 경우 모든 채팅을 받는다
                case VoteRole.Dead:
                    TextBox text3 = Instantiate(otherTextBoxPrefab, chatContent);
                    text3.SetMessage(player.Value, message);
                    textList.Add(text3);
                    break;
            }
        }
    }

    public void FindDeadBody()
    {
        // 시체 발견 해서 RPC로 모두에게 EmergencyReport 한다
        deadBodyFinder = true;
        photonView.RPC("EmergencyReport", RpcTarget.All, null);
    }


    [PunRPC]
    public void EmergencyReport()
    {
        // 긴급 보고
        TimeManager.Instance.TimeStop();
        SetUpPlayerState();
        AddAlivePlayerEntry();
        SetRole();
        skipVote.Initialized();

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
                // 죽었으면 사망자에 넣는다
                deadList.Add(player.Key);
            }
            
            if (TimeManager.Instance.isCurNight) // 밤
            {
                if (!PlayGameManager.Instance.playerList[player.Value.GetPlayerNumber()].isBird)
                { 
                    // 쥐를 참가자에 넣는다
                    participantList.Add(player.Key);
                }
                else
                {
                    // 새를 관전자에 넣는다
                    spectatorList.Add(player.Key);
                }
            }
            else // 낮
            {
                if (PlayGameManager.Instance.playerList[player.Value.GetPlayerNumber()].isBird)
                {
                    // 새를 참가자에 넣는다
                    participantList.Add(player.Key);
                }
                else
                {
                    // 쥐를 관전자에 넣는다
                    spectatorList.Add(player.Key);
                }
            }
        }
    }

    private void AddAlivePlayerEntry() // 참가자 플레이어 엔트리 추가
    {
        // playerVoteEntries 초기화
        for (int i = 0; i < playerVoteEntries.Count; i++)
        {
            Destroy(playerVoteEntries[i].gameObject);
        }

        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            // 해당 플레이어가 관전자면 실행 x
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
            // 만약 투표한 사람의 액터넘버가 엔트리의 액터넘버랑 똑같다면 투표완료 업데이트
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
        // 이미 투표를 했으면 리턴
        if (voteComplete)
            return;

        if (spectatorList.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
            return;

        if (deadList.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
            return;

        // 나 자신이면 리턴
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
            // 만약 투표한 사람의 액터넘버가 엔트리의 액터넘버랑 똑같다면 투표완료 업데이트
            if (entry.ActorNumber == actorNumber)
            {
                entry.CompleteVote();
            }

            // 타겟의 액터넘버가 엔트리의 엑터넘버랑 똑같다면 투표 수 상승
            if (entry.ActorNumber == target)
            {
                entry.AddVoteCount();
            }
        }


        // 투표 완료자들의 목록에 내가 없으면 추가
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
            // 관전자면 컨티뉴
            if (spectatorList.Contains(player.Key))
                continue;

            // 죽었으면 컨티뉴
            if (PlayGameManager.Instance.playerList[player.Value.GetPlayerNumber()].isDie)
                continue;

            alivePlayerCount++;
        }

        participantCount = alivePlayerCount;

        return participantCount;
    }

    public void VotingResult()
    {
        // 방장이 아니라면 여기서 리턴
        if (!PhotonNetwork.IsMasterClient)
            return;

        CalculateAlivePlayer();

        // 투표자 수가 참가자의 수보다 적으면 리턴
        if (voteCompletePlayerList.Count < participantCount)
            return;


        int mostVoterCount = 0;     // 득표수
        int mostVoterPlayer = -1;   // 최다 득표자

        foreach (PlayerVoteEntry entry in playerVoteEntries)
        {
            if (entry.VoteCount > mostVoterCount)
            {
                mostVoterCount = entry.VoteCount;
                mostVoterPlayer = entry.ActorNumber;
            }
        }

        // 최다 득표 수가 참가자의 절반 이상의 표를 받았으면 죽여버린다
        if (mostVoterCount >= participantCount / 2)
        {
            // 죽인다
            photonView.RPC("PlayerKill", RpcTarget.All, mostVoterPlayer);
        }
        else
        {
            // 아마 스킵
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
        // 투표 종료
        CheckGameOver();

        ResetText();
        voteWindow.SetActive(false);
        voteToDeathWindow.Init();
        deadBodyFinder = false;
        voteComplete = false;
        myRole = VoteRole.None;
        participantCount = 0;
        voteToDeathWindow.gameObject.SetActive(false);
        TimeManager.Instance.TimeResume();
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
        if (!PhotonNetwork.IsMasterClient)
            return;

        int spy = 0;
        int noneSpy = 0;

        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            // 관전자 제외
            if (spectatorList.Contains(player.Value.ActorNumber))
                continue;

            // 사망자 제외
            if (PlayGameManager.Instance.playerList[player.Value.GetPlayerNumber()].isDie)
                continue;

            // 스파이면 스파이 증가
            if (PlayGameManager.Instance.playerList[player.Value.GetPlayerNumber()].isSpy)
                spy++;
            // 일반 시민이면 시민 증가
            else
                noneSpy++;
        }

        //스파이가 없으면 시민 승리
        if (spy == 0)
        {
            ScoreManager.Instance.ActiveTimeOverNow();
        }
        // 스파이와 시민이 같은 인원이면 스파이팀 승리
        else if (spy == noneSpy)
        {
            ScoreManager.Instance.ActiveTimeOverNow();
        }
        // 둘다 아니면 게임 지속
        else
            return;
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
