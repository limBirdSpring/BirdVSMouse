using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Saebom
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ItemUsed : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public static Vector2 DefaultPos;
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();  
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            DefaultPos = this.transform.position;
            canvasGroup.blocksRaycasts = false;

        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            Vector2 currentPos = eventData.position;
            this.transform.position = currentPos;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.transform.position = DefaultPos;
            canvasGroup.blocksRaycasts = true;

        }
    }
}