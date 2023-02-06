using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : SingleTon<Inventory>
{
    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private List<ItemData>itemDatas = new List<ItemData>();

    private ItemData curSetItem;


    public void SetItem(ItemData item)
    {
        curSetItem = item;

        UpdateItemGFX();
    }

    public void DeleteItem()
    {
        curSetItem = null;
        itemImage.enabled = false;
    }

    private void UpdateItemGFX()
    {
        itemImage.sprite = curSetItem.itemIcon;
    }

    //해당 아이템이 장착되어있는지 확인하기
    public bool isItemSet(string name)
    {
        if (curSetItem.itemName == name)
            return true;

        return false;
    }
}
