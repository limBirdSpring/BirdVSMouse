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

    [HideInInspector]
    public bool isArrive = false;

    private void Awake()
    {
        manager = GetComponentInParent<RopeManager>();
    }

    private void OnEnable()
    {
        manager.GetRopeGamesCurState(ropeGames);
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

    public void SunOrMoonStart()
    {
        isArrive = false;
        if (TimeManager.Instance.isCurNight)
        {
            moon.StartMove();
        }
        else
        {
            sun.StartMove();
        }
    }

    public void SunOrMoonReset()
    {
        if (TimeManager.Instance.isCurNight)
        {
            moon.ResetPos();
        }
        else
        {
            sun.ResetPos();
        }
    }
}
