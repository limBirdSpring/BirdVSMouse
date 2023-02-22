using Saebom;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Emergency : MonoBehaviour
{
    public void Emnergency()
    {
        
        if (PlayGameManager.Instance.myPlayerState.isBird && !TimeManager.Instance.isCurNight && MissionButton.Instance.birdEmergency > 0)
        {
            MissionButton.Instance.birdEmergency--;
            MissionButton.Instance.MasterSetEmergency();
            VoteManager.Instance.FindDeadBody();
        }
        else if (!PlayGameManager.Instance.myPlayerState.isBird && TimeManager.Instance.isCurNight && MissionButton.Instance.mouseEmergency > 0)
        {
            MissionButton.Instance.mouseEmergency--;
            MissionButton.Instance.MasterSetEmergency();
            VoteManager.Instance.FindDeadBody();
        }

    }

}
