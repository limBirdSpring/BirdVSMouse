using Saebom;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class RopeController : MonoBehaviour
{
    public RopeGame[] ropeGames;

    public SunOrMoon sun;
    public SunOrMoon moon;

    private RopeManager manager;

    private void Awake()
    {
        //ropeGames = GetComponentsInChildren<RopeGame>();
        manager = GetComponentInParent<RopeManager>();
    }

    private void OnEnable()
    {
        manager.GetRopeGamesCurState(ropeGames);
        LoadUIRPC();
    }

    private void OnDisable()
    {
        manager.SetRopeGamesCurState(ropeGames);
        manager.PlayerUpdateCurMission();
    }

    public void ResetRope()
    {
        for (int i = 0; i < ropeGames.Length; i++)
        {
            ropeGames[i].RopeReset();
        }
    }

    public void LoadUIRPC()
    {
        for (int i = 0; i < ropeGames.Length; i++)
        {
            ropeGames[i].UpdateUI();
        }
    }

    /*public void SaveUIRPC(RopeGame[] ropes)
    {
        for (int i = 0; i < ropeGames.Length; i++)
        {
            ropeGames[i].curState = ropes[i].curState;
        }
    }*/

    public void SunOrMoonStart()
    {
        if (TimeManager.Instance.isCurNight)
        {
            moon.StartMove();
        }
        else
        {
            sun.StartMove();
        }
    }
}
