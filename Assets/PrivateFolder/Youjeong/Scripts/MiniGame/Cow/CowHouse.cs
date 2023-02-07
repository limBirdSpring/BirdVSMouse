using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class CowHouse : MonoBehaviour,IDropHandler
{
    private CowManager manager;

    [SerializeField]
    private bool isBrid;
    [SerializeField]
    private Inventory inventory;

    public bool isBridHouse { get; private set; }
    private Cow[] Cows;
    private bool isCow;

    private void Awake()
    {
        isBridHouse = isBrid;
        Cows = GetComponentsInChildren<Cow>();
        manager=GetComponentInParent<CowManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        isCow = inventory.isItemSet("Cow");
        if (!isCow)
            return;
        for(int i=0;i<Cows.Length;i++) 
        {
            if (!Cows[i].gameObject.activeSelf)
            {
                Cows[i].gameObject.SetActive(true);
                manager.AddCow(isBridHouse);
                inventory.DeleteItem();
                return;
            }

        }
    }
}
