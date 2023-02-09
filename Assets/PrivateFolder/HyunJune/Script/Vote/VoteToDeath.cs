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
        // ����
        if (state.isBird)
        {
            birdVote.enabled = true;
            target.sprite = state.sprite;
        }
        // ���
        else
        {
            birdVote.enabled = true;
            target.sprite = state.sprite;
        }
    }

    public void SwitchForm()
    {
        target.enabled = false;
        // ��
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
        // ��
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
            team = "��";
        }
        else
        {
            team = "��";
        }

        // �����̿�����
        if (state.isSpy)
        {
            message.text = string.Format("{0}�� {1}�� �����̿����ϴ�...!", team, state.name);
            message.enabled = true;
        }
        // ������ �ƴϸ�
        else
        {
            message.text = string.Format("{0}�� {1}�� �����̰� �ƴϿ����ϴ�...!", team, state.name);
            message.enabled = true;
        }
    }
}
