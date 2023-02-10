using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkipVote : MonoBehaviour
{
    [SerializeField]
    private TMP_Text voteCountUI;

    private Button voteButton;

    private int voteCount;

    private void Awake()
    {
        voteButton = GetComponent<Button>();
        voteButton.onClick.AddListener(OnSkipPressed);
    }

    public int VoteCount
    {
        get { return voteCount; }
    }

    public void Initialized()
    {
        voteCount = 0;
        voteCountUI.text = voteCount.ToString();
        voteButton.interactable = true;
    }

    public void AddSkipVoteCount()
    {
        voteCount++;
        voteCountUI.text = voteCount.ToString();
    }

    public void OnSkipPressed()
    {
        VoteManager.Instance.VoteSkip();
    }

    public void ToggleButton(bool toggle)
    {
        voteButton.interactable = toggle;
    }
}
