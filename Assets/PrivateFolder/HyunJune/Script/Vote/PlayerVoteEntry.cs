using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Saebom;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVoteEntry : MonoBehaviourPun
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private Image playerIcon;
    [SerializeField] private TMP_Text voteCountUI;
    [SerializeField] private Image finder;
    [SerializeField] private Image voteTarget;

    [SerializeField]
    private Button voteButton;

    private int actorNumber;
    public int ActorNumber 
    { 
        get { return actorNumber; }    
    }

    private int voteCount;

    public int VoteCount
    {
        get { return voteCount; }
    }

    private void Awake()
    {
        //voteButton = GetComponent<Button>();
        voteButton.onClick.AddListener(OnVotePressed);
    }

    public void OnVotePressed()
    {
        VoteManager.Instance.Vote(actorNumber);
    }

    public void Initialized(Photon.Realtime.Player player)
    {
        actorNumber = player.ActorNumber;
        playerName.text = PlayGameManager.Instance.playerList[player.GetPlayerNumber()].name;
        playerIcon.sprite = PlayGameManager.Instance.playerList[player.GetPlayerNumber()].sprite;
        voteCount = 0;
        voteCountUI.text = voteCount.ToString();
        finder.enabled = false;
        voteTarget.enabled = false;

        if (PlayGameManager.Instance.playerList[player.GetPlayerNumber()].isDie)
        {
            ToggleButton(false);
        }

        if (actorNumber == VoteManager.Instance.deadBodyFinderActNum)
        {
            finder.enabled = true;
        }
            
    }

    [PunRPC]
    public void AddVoteCount()
    {
        voteCount++;
        voteCountUI.text = voteCount.ToString();
    }

    public void DeadSetting()
    {
        // 흑백 또는 X표시로 사망 표시 및 투표 불가
        ToggleButton(false);
    }

    public void SetVoteTarget()
    {
        voteTarget.enabled = true;
    }

    public void ToggleButton(bool toggle)
    {
        voteButton.interactable = toggle;
    }
}
