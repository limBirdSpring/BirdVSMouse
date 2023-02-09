using Saebom;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class RopeController : MonoBehaviour
{
    private RopeGame[] ropeGames;

    public SunOrMoon sun;
    public SunOrMoon moon;

    public void ResetRope()
    {
        ropeGames = GetComponentsInChildren<RopeGame>();

        for (int i = 0; i < ropeGames.Length; i++)
        {
            ropeGames[i].RopeReset();
        }
    }

    public void LoadUIRPC()
    {
        ropeGames = GetComponentsInChildren<RopeGame>();

        for (int i = 0; i < ropeGames.Length; i++)
        {
            ropeGames[i].UpdateUI();
        }
    }

    public void SaveUIRPC(RopeGame[] ropes)
    {
        ropeGames = GetComponentsInChildren<RopeGame>();

        for (int i = 0; i < ropeGames.Length; i++)
        {
            ropeGames[i].curState = ropes[i].curState;
        }
    }

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
