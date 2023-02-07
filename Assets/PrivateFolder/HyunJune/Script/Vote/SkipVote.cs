using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkipVote : MonoBehaviour
{
    [SerializeField]
    private TMP_Text voteCountUI;

    private int voteCount;

    public int VoteCount
    {
        get { return voteCount; }
    }

    public void Initialized()
    {
        voteCount = 0;
        voteCountUI.text = voteCount.ToString();
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
}
