using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Coupon : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField coupon;

    public void OnButtonClick()
    {
        if (coupon.text == "김토리바보")
        {
            //아이템 얻기
        }
    }
}
