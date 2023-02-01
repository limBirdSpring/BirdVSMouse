using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : SingleTon<Inventory>
{
    private ItemData curSetItem;

    [SerializeField]
    private Image itemImage;

    public void SetItem(ItemData item)
    {
        curSetItem = item;

        UpdateItemGFX();
    }

    private void UpdateItemGFX()
    {
        itemImage.sprite = curSetItem.sprite;
    }

    //해당 아이템이 장착되어있는지 확인하기
    public bool isItemSet(string name)
    {
        return false;
    }
}
