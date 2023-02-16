using Saebom;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VoteToDeath : MonoBehaviour
{
    [SerializeField]
    private Image birdVote;
    [SerializeField]
    private Image mouseVote;
    [SerializeField]
    private Image spyBird;
    [SerializeField]
    private Image spyMouse;
    [SerializeField]
    private Image ghostBird;
    [SerializeField]
    private Image ghostMouse;
    [SerializeField]
    private Image target;
    [SerializeField]
    private TMP_Text message;

    private Saebom.PlayerState state;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        if (state.isBird)
        {
            birdVote.enabled = true;
            target.sprite = state.sprite;
            target.enabled = true;
        }
        else
        {
            mouseVote.enabled = true;
            target.sprite = state.sprite;
            target.enabled = true;
        }
        SoundManager.Instance.PlayUISound(UISFXName.VoteDie);
    }

    public void Init()
    {
        birdVote.enabled = false;
        mouseVote.enabled = false;
        spyBird.enabled = false;
        spyMouse.enabled = false;
        ghostBird.enabled = false;
        ghostMouse.enabled = false;
        target.enabled = false;
        message.enabled = false;
    }

    public void Setting(Saebom.PlayerState state)
    {
        this.state = state;
    }

    public void SwitchForm()
    {
        target.enabled = false;
        // 새
        if (state.isBird)
        {
            if (state.isSpy)
            {
                spyMouse.enabled = true;
            }
            else
            {
                ghostBird.enabled = true;
            }

        }
        // 쥐
        else
        {
            if (state.isSpy)
            {
                spyBird.enabled = true;
            }
            else
            {
                ghostMouse.enabled = true;
            }
        }
    }

    public void ShowResult()
    {
        string team;
        if (state.isBird)
        {
            team = "새";
        }
        else
        {
            team = "쥐";
        }

        // 스파이였으면
        if (state.isSpy)
        {
            message.text = string.Format("{0}팀 {1}, 그는 스파이였습니다...!", team, state.name);
            message.enabled = true;
        }
        // 스파이 아니면
        else
        {
            message.text = string.Format("{0}팀 {1}, 그는 스파이가 아니였습니다...!", team, state.name);
            message.enabled = true;
        }
    }
}
