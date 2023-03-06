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
        private AudioSource audioWaterDrop;

       
        private bool isWater;

        public void OnDrop(PointerEventData eventData)
        {
            isWater = Inventory.Instance.isItemSet("Water");
            if (!isWater)
                return;
            Inventory.Instance.DeleteItem();
            game.PlusWater();
            AudioPlay();
        }

        private void AudioPlay()
        {
            audioWaterDrop.Play();
        }
    }
}

