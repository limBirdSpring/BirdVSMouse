using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : SingleTon<Inventory>
{
    [SerializeField]
    private List<ItemData> itemDatas = new List<ItemData>();

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
        itemImage.sprite = curSetItem.itemIcon;
    }

    //�ش� �������� �����Ǿ��ִ��� Ȯ���ϱ�
    public bool isItemSet(string name)
    {
        foreach (ItemData item in itemDatas)
        {
            if (item.itemName == name)
                return true;
        }
        return false;
    }
}
