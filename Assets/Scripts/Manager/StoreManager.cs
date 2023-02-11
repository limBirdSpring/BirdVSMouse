using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    [SerializeField]
    private CollectionItem mainItem;

    [SerializeField]
    private string mainText;

    [SerializeField]
    private CollectionItem photoItem1;

    [SerializeField]
    private CollectionItem photoItem2;

    [SerializeField]
    private CollectionItem photoItem3;

    [SerializeField]
    private CollectionItem badgeItem1;

    [SerializeField]
    private CollectionItem badgeItem2;

    [SerializeField]
    private CollectionItem badgeItem3;

    //=========================================

    [SerializeField]
    private Image mainImg;

    [SerializeField]
    private TextMeshProUGUI mainPrice;

    [SerializeField]
    private TextMeshProUGUI mainName;


    [SerializeField]
    private Image photo1Img;

    [SerializeField]
    private TextMeshProUGUI photo1Price;

    [SerializeField]
    private TextMeshProUGUI photo1Name;

    [SerializeField]
    private Image photo2Img;

    [SerializeField]
    private TextMeshProUGUI photo2Price;

    [SerializeField]
    private TextMeshProUGUI photo2Name;

    [SerializeField]
    private Image photo3Img;

    [SerializeField]
    private TextMeshProUGUI photo3Price;

    [SerializeField]
    private TextMeshProUGUI photo3Name;


    [SerializeField]
    private Image badge1Img;

    [SerializeField]
    private TextMeshProUGUI badge1Price;

    [SerializeField]
    private TextMeshProUGUI badge1Name;

    [SerializeField]
    private Image badge2Img;

    [SerializeField]
    private TextMeshProUGUI badge2Price;

    [SerializeField]
    private TextMeshProUGUI badge2Name;

    [SerializeField]
    private Image badge3Img;

    [SerializeField]
    private TextMeshProUGUI badge3Price;

    [SerializeField]
    private TextMeshProUGUI badge3Name;


    private void OnEnable()
    {
        SetGoods();
    }

    private void SetGoods()
    {

        mainImg.sprite = mainItem.itemIcon;
        mainPrice.text = "\\" + mainItem.price.ToString();
        mainName.text = mainText;


 
        photo1Img.sprite = photoItem1.itemIcon;
        photo1Price.text = "\\" + photoItem1.price.ToString();
        photo1Name.text = photoItem1.itemName;

 
        photo2Img.sprite = photoItem2.itemIcon;
        photo2Price.text = "\\" + photoItem2.price.ToString();
        photo2Name.text = photoItem2.itemName;


        photo3Img.sprite = photoItem3.itemIcon;
        photo3Price.text = "\\" + photoItem3.price.ToString();
        photo3Name.text = photoItem3.itemName;

        badge1Img.sprite = badgeItem1.itemIcon;
        badge1Price.text = "\\" + badgeItem1.price.ToString();
        badge1Name.text = badgeItem1.itemName;


        badge2Img.sprite = badgeItem2.itemIcon;
        badge2Price.text = "\\" + badgeItem2.price.ToString();
        badge2Name.text = badgeItem2.itemName;

  
        badge3Img.sprite = badgeItem3.itemIcon;
        badge3Price.text = "\\" + badgeItem3.price.ToString();
        badge3Name.text = badgeItem3.itemName;



    }
    


    public void MainOnClickItem()
    {
        
    }

    public void Photo1OnClickItem()
    {

    }
}
