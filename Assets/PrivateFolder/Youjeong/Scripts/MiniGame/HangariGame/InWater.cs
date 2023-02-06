using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InWater : MonoBehaviour
{
    private Image inWater;
    
    void Start()
    {
        inWater = GetComponent<Image>();
    }

    public void FillWater(float amount)
    {
        inWater.fillAmount = amount / 100f;
    }
}
