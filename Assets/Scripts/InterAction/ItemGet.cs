using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ش� ������Ʈ�� ������ ������Ʈ ��ó �ݸ����� �߰����Ѽ� ����Ѵ�.


public class ItemGet : MonoBehaviour
{
    //�ش� ��ҿ��� ���� ������
    [SerializeField]
    private ItemData itemData;

    public void GetItem()
    {
        Item item = new Item();
        item.data = itemData;
        Inventory.Instance.SetItem(item);
    }
}
