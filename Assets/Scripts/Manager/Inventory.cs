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

    [SerializeField]
    private ItemData curSetItem;


    public void SetItem(ItemData item)
    {
        curSetItem = item;
        itemImage.enabled = true;
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

    //�ش� �������� �����Ǿ��ִ��� Ȯ���ϱ�
    public bool isItemSet(string name)
    {
        if (curSetItem == null)
            return false;
        else if (curSetItem.itemName == name)
            return true;

        return false;
    }
}
