using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;

public class Cow : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private ItemData cowData;

    private CowManager manager;
    private CowHouse house;
    private bool isBirdHouse;

    private void Awake()
    {
        manager = GetComponentInParent<CowManager>();
        house = GetComponentInParent<CowHouse>();
        isBirdHouse = house.isBridHouse;
    }

    public void GetCow()
    {
        if (isBirdHouse == PlayGameManager.Instance.myPlayerState.isBird && !PlayGameManager.Instance.myPlayerState.isSpy||Inventory.Instance.isItemSet("Cow"))
            return;
        inventory.SetItem(cowData);
        manager.DeleteCow(isBirdHouse);
        this.gameObject.SetActive(false);
    }
}
