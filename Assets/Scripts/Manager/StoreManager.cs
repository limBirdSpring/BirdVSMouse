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

    private void OnEnable()
    {
        SetGoods();
    }

    private void SetGoods()
    {
        GameObject main = GameObject.Find("Sale");
        main.transform.GetChild(0).GetComponent<Image>().sprite = mainItem.itemIcon;
        main.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "\\" + mainItem.price.ToString();
        main.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = mainText;


        GameObject photo1 = GameObject.Find("Photo1");
        photo1.transform.GetChild(0).GetComponent<Image>().sprite = photoItem1.itemIcon;
        photo1.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "\\" + photoItem1.price.ToString();
        photo1.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = photoItem1.name;

        GameObject photo2 = GameObject.Find("Photo2");
        photo2.transform.GetChild(0).GetComponent<Image>().sprite = photoItem2.itemIcon;
        photo2.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "\\" + photoItem2.price.ToString();
        photo2.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = photoItem2.name;

        GameObject photo3 = GameObject.Find("Photo3");
        photo3.transform.GetChild(0).GetComponent<Image>().sprite = photoItem3.itemIcon;
        photo3.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "\\" + photoItem3.price.ToString();
        photo3.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = photoItem3.name;

        GameObject badge1 = GameObject.Find("Badge1");
        badge1.transform.GetChild(0).GetComponent<Image>().sprite = badgeItem1.itemIcon;
        badge1.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "\\" + badgeItem1.price.ToString();
        badge1.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = badgeItem1.name;

        GameObject badge2 = GameObject.Find("Badge2");
        badge2.transform.GetChild(0).GetComponent<Image>().sprite = badgeItem2.itemIcon;
        badge2.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "\\" + badgeItem2.price.ToString();
        badge2.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = badgeItem2.name;

        GameObject badge3 = GameObject.Find("Badge3");
        badge3.transform.GetChild(0).GetComponent<Image>().sprite = badgeItem3.itemIcon;
        badge3.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "\\" + badgeItem3.price.ToString();
        badge3.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = badgeItem3.name;



    }
    


    public void MainOnClickItem()
    {
        
    }

    public void Photo1OnClickItem()
    {

    }
}
