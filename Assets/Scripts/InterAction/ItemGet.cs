using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UIElements;

//해당 컴포넌트는 아이템 오브젝트 근처 콜리더에 추가시켜서 사용한다.


public class ItemGet : MonoBehaviour
{
    //해당 장소에서 얻을 아이템
    [SerializeField]
    private ItemData itemData;

    public void GetItem()
    {
        Inventory.Instance.SetItem(itemData);
    }
}
