using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Youjeong;

[RequireComponent(typeof(Image))]
public class InWater : MonoBehaviour
{
    [SerializeField]
    private HagnariGame game;
    [SerializeField]
    private HangariManager manager;
    [SerializeField]
    private TextMeshProUGUI amount;

    private Image inWater;
    private Coroutine fillWaterCoroutine;
    private float cooltime;
    private float finalAmount;
    private float curAmount;
    private float gapAmount;

    void Start()
    {
        inWater = GetComponent<Image>();
        SetWaterImage();
    }
   
    private void OnDisable()
    {
        SetWaterImage();
    }

    public void SetWaterImage()
    {
        if(fillWaterCoroutine!= null)
            StopCoroutine(fillWaterCoroutine);
        cooltime = 0;
        curAmount = manager.waterAmount;
        inWater.fillAmount = curAmount / 90;
        SetAmountText();
    }

    public void ChangeWater(float amount)
    {
        if (fillWaterCoroutine != null)
            StopCoroutine(fillWaterCoroutine);
        finalAmount = amount;
        gapAmount = (finalAmount - curAmount);
        fillWaterCoroutine = StartCoroutine("FillWater");
    }

    private IEnumerator FillWater()
    {
        cooltime = 0;
        while (cooltime< game.delay)
        {
            
            cooltime += 0.1f;
            curAmount += gapAmount * (0.1f / game.delay);
            curAmount = curAmount > 90 ? 90 : curAmount;
            curAmount = curAmount < 0 ? 0 : curAmount;
            inWater.fillAmount = curAmount / 90;
            SetAmountText();
            if (cooltime >= game.delay)
            {
                SetWaterImage();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void SetAmountText()
    {
        amount.text = Mathf.Floor( curAmount).ToString();
    }
}
