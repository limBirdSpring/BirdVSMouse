using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;

namespace Youjeong
{
    public class Cow : MonoBehaviour
    {
        [SerializeField]
        private ItemData cowData;

        private CowManager manager;
        private CowHouse house;
        private bool isBirdHouse;

        private void Awake()
        {
            manager = GetComponentInParent<CowManager>();
            house = GetComponentInParent<CowHouse>();
            isBirdHouse = house.isBirdHouse;
        }

        public void GetCow()
        {
            if (isBirdHouse == PlayGameManager.Instance.myPlayerState.isBird && !PlayGameManager.Instance.myPlayerState.isSpy || Inventory.Instance.isItemSet("Cow"))
                return;
            Inventory.Instance.SetItem(cowData);
            manager.DeleteCow(isBirdHouse);
            this.gameObject.SetActive(false);
        }
    }
}

