using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : SingleTon<Inventory>
{
    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private List<Item> itemDatas = new List<Item>();

    private Item curSetItem;


    public void SetItem(Item item)
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
        itemImage.sprite = curSetItem.data.itemIcon;
    }

    //�ش� �������� �����Ǿ��ִ��� Ȯ���ϱ�
    public bool isItemSet(string name)
    {
        foreach (Item item in itemDatas)
        {
            if (item.data.itemName == name)
                return true;
        }
        return false;
    }
}
