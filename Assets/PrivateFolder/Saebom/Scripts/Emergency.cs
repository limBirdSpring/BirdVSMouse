using Saebom;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Emergency : MonoBehaviour
{

    private bool active = false;

    public void Emnergency()
    {
        
        if (active == false && PlayGameManager.Instance.myPlayerState.isBird && !TimeManager.Instance.isCurNight && MissionButton.Instance.birdEmergency > 0)
        {
            MissionButton.Instance.birdEmergency--;
            MissionButton.Instance.MasterSetEmergency();
            VoteManager.Instance.FindDeadBody();
            active = true;
            StartCoroutine(Cor());
        }
        else if (active == false && !PlayGameManager.Instance.myPlayerState.isBird && TimeManager.Instance.isCurNight && MissionButton.Instance.mouseEmergency > 0)
        {
            MissionButton.Instance.mouseEmergency--;
            MissionButton.Instance.MasterSetEmergency();
            VoteManager.Instance.FindDeadBody();
            active = true;
            StartCoroutine(Cor());
        }

    }

    private IEnumerator Cor()
    {
        yield return new WaitForSeconds(2f);
        active = false;
    }

}
