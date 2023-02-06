using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HagnariGame : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private Inventory inventory;

    private ItemData itemData;


    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
