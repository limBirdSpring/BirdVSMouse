using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Saebom;

namespace Youjeong
{
    public class Frog : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private HagnariGame game;
        [SerializeField]
        private GameObject sprayWater;

        private RectTransform rectTransform;
        

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(!game.isOutHangari&& PlayGameManager.Instance.myPlayerState.isSpy)
            {
                game.isOutHangari = true;
                game.MinusWater();
                sprayWater.SetActive(true);
                OutFrog();
            }
            else if(game.isOutHangari)
            {
                game.isOutHangari = false;
                sprayWater.SetActive(false);
                InFrog();
            }
        }

        private void OutFrog()
        {
            rectTransform.position += new Vector3(600, 0, 0);
        }

        private void InFrog()
        {
            rectTransform.position -= new Vector3(600, 0, 0);
        }
       
    }
}

