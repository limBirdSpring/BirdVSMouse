using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UIElements;

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
