using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Youjeong
{
    public class Hagnari : MonoBehaviour, IDropHandler//, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private HagnariGame game;

        [SerializeField]
        private Inventory inventory;

        private bool isWater;

        public void OnDrop(PointerEventData eventData)
        {
            isWater = inventory.isItemSet("Water");
            Debug.Log(isWater);
            if (!isWater)
                return;
            inventory.DeleteItem();
            game.PlusWater();
        }
    }
}

