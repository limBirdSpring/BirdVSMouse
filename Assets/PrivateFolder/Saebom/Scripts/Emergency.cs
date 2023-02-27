using Saebom;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Emergency : MonoBehaviour
{

    private bool active = false;

    private void Emnergency()
    {
        
        if (active == false && PlayGameManager.Instance.myPlayerState.isBird && !TimeManager.Instance.isCurNight && MissionButton.Instance.birdEmergency > 0)
        {
            Debug.Log("새 이머젼시 부름");
            VoteManager.Instance.FindDeadBody();
            active = true;
            StartCoroutine(Cor());
        }
        else if (active == false && !PlayGameManager.Instance.myPlayerState.isBird && TimeManager.Instance.isCurNight && MissionButton.Instance.mouseEmergency > 0)
        {

            Debug.Log("쥐 이머젼시 부름");
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
