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

    //�ش� �������� �����Ǿ��ִ��� Ȯ���ϱ�
    public bool isItemSet(string name)
    {
        return false;
    }
}
