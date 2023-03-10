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

    public int deadBodyFinderActNum;
    private bool voteComplete = false;
    private float participantCount;
    private VoteRole myRole;

    [Header("SkipWindow")]
    [SerializeField]
    private ResultNothing skipWindow;

    [Header("StartVoteWindow")]
    [SerializeField]
    private GameObject startWindow;

    private IEnumerator co;

    private void Awake()
    {
        Instance = this;
        //FindObjectsOfType<PlayerController>();
        chatInputField.characterLimit = 50;
        controllers = FindObjectsOfType<PlayerControllerTest>();
        deadBodyFinderActNum = -1;
    }

    // ?????? ????
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

            photonView.RPC("UpdateTime", RpcTarget.All, time);
            yield return null;
        }

        Debug.Log("????????");
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
            // ???????? ????
            if (spectatorList.Contains(player.Value.ActorNumber))
                return;

            // ???????? ????
            if (deadList.Contains(player.Value.ActorNumber))
                return;

            // ?????? ?????? ????
            if (voteCompletePlayerList.Contains(player.Value.ActorNumber))
                return;


            // ???? ???? ????
            VoteSkip();
        }
    }

    [PunRPC]
    private void SendMessage(string message, int actorNumeber)
    {

        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            // ?????? ?????? ?????? ?????? ?? ?????????? ???? ???? ????
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

        // ?????? ???? ?????? ???? ???????? ???? ?????? ???? ?????? ????
        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (actorNumeber != player.Value.ActorNumber)
                continue;

            switch (myRole)
            {
                // ???? ???????? ????
                case VoteRole.Participant:
                    // ???? ???? ?????????? ???????? ?????? ??????
                    if (participantList.Contains(actorNumeber) && !deadList.Contains(actorNumeber))
                    {
                        TextBox text = Instantiate(otherTextBoxPrefab, chatContent);
                        text.SetMessage(player.Value, message);
                        SoundManager.Instance.PlayUISound(UISFXName.Chat);
                        textList.Add(text);
                    }
                    break;
                // ???? ???????? ???? ???????? ????
                case VoteRole.Spectator:
                    // ???? ???? ?????????? ?????? ?? ???? ????????
                    if (deadList.Contains(actorNumeber))
                        return;
                             
                    TextBox text2 = Instantiate(otherTextBoxPrefab, chatContent);
                    text2.SetMessage(player.Value, message);
                    SoundManager.Instance.PlayUISound(UISFXName.Chat);
                    textList.Add(text2);
                    break;
                // ???? ???????? ???? ???? ?????? ??????
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
        // ???? ???? ???? RPC?? ???????? EmergencyReport ????
        if ((PlayGameManager.Instance.myPlayerState.isBird && !TimeManager.Instance.isCurNight) ||
           (!PlayGameManager.Instance.myPlayerState.isBird && TimeManager.Instance.isCurNight))
        {
            deadBodyFinderActNum = PhotonNetwork.LocalPlayer.ActorNumber;
            photonView.RPC("EmergencyReport", RpcTarget.All, deadBodyFinderActNum);

        }
    }


    [PunRPC]
    public void EmergencyReport(int finderNum)
    {
        SoundManager.Instance.PlayUISound(UISFXName.Vote);
        startWindow.gameObject.SetActive(true);

        // ???? ????
        SetUpPlayerState();
        AddAlivePlayerEntry(finderNum);
        SetRole();
        skipVote.Initialized();
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            TimeManager.Instance.TimeStop();

        StartCoroutine(OpenVoteWindow());
    }

    private IEnumerator OpenVoteWindow()
    {
        yield return new WaitForSeconds(5);
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            co = StartTimer();
            StartCoroutine(co);
        }
        voteWindow.gameObject.SetActive(true);
        startWindow.gameObject.SetActive(false);
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
                // ???????? ???????? ??????
                deadList.Add(player.Key);
            }
            
            if (TimeManager.Instance.isCurNight) // ??
            {
                if (!PlayGameManager.Instance.playerList[player.Value.GetPlayerNumber()].isBird)
                { 
                    // ???? ???????? ??????
                    participantList.Add(player.Key);
                }
                else
                {
                    // ???? ???????? ??????
                    spectatorList.Add(player.Key);
                }
            }
            else // ??
            {
                if (PlayGameManager.Instance.playerList[player.Value.GetPlayerNumber()].isBird)
                {
                    // ???? ???????? ??????
                    participantList.Add(player.Key);
                }
                else
                {
                    // ???? ???????? ??????
                    spectatorList.Add(player.Key);
                }
            }
        }
    }

    private void AddAlivePlayerEntry(int finderNum) // ?????? ???????? ?????? ????
    {
        // playerVoteEntries ??????
        for (int i = 0; i < playerVoteEntries.Count; i++)
        {
            Destroy(playerVoteEntries[i].gameObject);
        }
        playerVoteEntries.Clear();

        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            // ???? ?????????? ???????? ???? x
            if (spectatorList.Contains(player.Key))
                continue;

            PlayerVoteEntry entry = Instantiate(voteEntryPrefab, entryContent);
            entry.Initialized(player.Value, finderNum);
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
            // ???? ?????? ?????? ?????????? ???????? ?????????? ???????? ???????? ????????
            if (entry.ActorNumber == actorNumber)
            {
                //entry.CompleteVote();
                voteCompletePlayerList.Add(entry.ActorNumber);
            }
        }

        VotingResult();
    }

    public void Vote(int target)
    {
        // ???? ?????? ?????? ????
        if (voteComplete)
            return;

        if (spectatorList.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
            return;

        if (deadList.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
            return;

        // ???????? ???? ???? On  /  ???? ???? ?? ???? ?????? ??????????.
        //if (PhotonNetwork.LocalPlayer.ActorNumber == target)
        //    return;

        voteComplete = true;
        ToggleAllButton(false);
        foreach (PlayerVoteEntry entry in playerVoteEntries)
        {
            if (entry.ActorNumber == target)
            {
                entry.SetVoteTarget();
            }
        }
        photonView.RPC("VoteCheckRPC", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, target);
    }

    [PunRPC]
    public void VoteCheckRPC(int actorNumber, int target)
    {
        foreach (PlayerVoteEntry entry in playerVoteEntries)
        {
            // ?????? ?????????? ???????? ?????????? ???????? ???? ?? ????
            if (entry.ActorNumber == target)
            {
                entry.AddVoteCount();
            }
        }


        // ???? ?????????? ?????? ???? ?????? ????
        if (!voteCompletePlayerList.Contains(actorNumber))
        {
            voteCompletePlayerList.Add(actorNumber);
        }

        VotingResult();
    }

    public float CalculateAlivePlayer()
    {
        int alivePlayerCount = 0;
        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            // ???????? ??????
            if (spectatorList.Contains(player.Key))
                continue;

            // ???????? ??????
            if (PlayGameManager.Instance.playerList[player.Value.GetPlayerNumber()].isDie)
                continue;

            alivePlayerCount++;
        }

        participantCount = alivePlayerCount;

        return participantCount;
    }

    public void VotingResult()
    {
        // ?????? ???????? ?????? ????
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            return;

        CalculateAlivePlayer();

        // ?????? ???? ???????? ?????? ?????? ????
        if (voteCompletePlayerList.Count < participantCount)
            return;


        int mostVoterCount = 0;     // ??????
        int mostVoterPlayer = -1;   // ???? ??????

        foreach (PlayerVoteEntry entry in playerVoteEntries)
        {
            if (entry.VoteCount > mostVoterCount)
            {
                mostVoterCount = entry.VoteCount;
                mostVoterPlayer = entry.ActorNumber;
            }
        }
        
        // ???? ???? ???? ???????? ???? ?????? ???? ???????? 
        if (mostVoterCount >= participantCount / 2 && mostVoterCount > skipVote.VoteCount)
        {
            // ??????
            photonView.RPC("PlayerKill", RpcTarget.All, mostVoterPlayer);
        }
        else
        {
            // ???? ????
            photonView.RPC("NothingWindowOpen", RpcTarget.All, null);
        }
    }

    [PunRPC]
    private void NothingWindowOpen()
    {
        skipWindow.gameObject.SetActive(true);
        SoundManager.Instance.PlayUISound(UISFXName.VoteDie);
        StartCoroutine(VotingEnd(5));
    }

    [PunRPC]
    private void PlayerKill(int actorNumber)
    {
        // ???? ?????? ????
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            // ?????????? ???? ??????????
            controllers = FindObjectsOfType<PlayerControllerTest>();
            // ?????????? for?????? ??????
            for (int i = 0; i < controllers.Length; i++)
            {
                // ???? ?????? ?????????? ?????? ?????????? ???? ???? ?????????? ???? ????
                if (controllers[i].photonView.OwnerActorNr == actorNumber)
                {
                    // ??????
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
        StartCoroutine(VotingEnd(10));
    }

    public IEnumerator VotingEnd(int time)
    {
        yield return new WaitForSeconds(time);
        photonView.RPC("VotingEndRPC", RpcTarget.All, null);
    }

    [PunRPC]
    private void VotingEndRPC()
    {
        // ???? ????
        StopAllCoroutines();

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            TimeManager.Instance.TimeResume();
            CheckGameOver();
        }

        ResetText();
        voteWindow.SetActive(false);
        voteToDeathWindow.Init();
        deadBodyFinderActNum = -1;
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
            // ?????? ????
            if (spectatorList.Contains(player.Value.ActorNumber))
                continue;

            // ?????? ????
            if (PlayGameManager.Instance.playerList[player.Value.GetPlayerNumber()].isDie)
                continue;

            // ???????? ?????? ????
            if (PlayGameManager.Instance.playerList[player.Value.GetPlayerNumber()].isSpy)
                spy++;
            // ???? ???????? ???? ????
            else
                noneSpy++;
        }

        //???????? ?????? ???? ????
        if (spy == 0)
        {
            if (PhotonNetwork.IsMasterClient)
                GameOver();
        }
        // ???????? ?????? ???? ???????? ???????? ????
        else if (spy == noneSpy)
        {
            if (PhotonNetwork.IsMasterClient)
                GameOver();
        }
        // ???? ?????? ???? ????
        else
            return;
    }

    private void GameOver()
    {
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
