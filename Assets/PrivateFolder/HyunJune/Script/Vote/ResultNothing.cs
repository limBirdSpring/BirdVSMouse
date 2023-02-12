using Saebom;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultNothing : MonoBehaviour
{
    [SerializeField]
    private Image dayTime;
    [SerializeField]
    private Image night;
    [SerializeField]
    private TMP_Text message;


    private void OnEnable()
    {
        // ���̸�
        if (TimeManager.Instance.isCurNight)
        {
            dayTime.enabled = false;
            night.enabled = true;
        }
        // ���̸�
        else
        {
            night.enabled = false;
            dayTime.enabled = true;
        }
    }
}
