using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HyunJune
{
    [Flags]
    public enum CurColor
    {
        None    = 0000_0000_0000_0000,
        White   = 0000_0000_0000_0001,
        Red     = 0000_0000_0000_0010,
        Blue    = 0000_0000_0000_0100,
        Yellow  = 0000_0000_0000_1000,
        Black   = White | Red | Blue | Yellow
    }



    public class Cloth : MonoBehaviour, IDropHandler
    {
        private Image cloth;
        Dictionary<CurColor, Color32> dicColor;

        public CurColor curColor;

        private void Awake()
        {
            cloth = GetComponent<Image>();
            dicColor = new Dictionary<CurColor, Color32>();
        }

        private void Start()
        {
            curColor = CurColor.None;
            Init();
        }

        private void Init()
        {
            dicColor.Add(CurColor.None, new Color32(224, 216, 192, 255));

            dicColor.Add(CurColor.White, new Color32(255, 255, 255, 255));
            dicColor.Add(CurColor.Red, new Color32(255, 0, 0, 255));
            dicColor.Add(CurColor.Blue, new Color32(46, 86, 227, 255));
            dicColor.Add(CurColor.Yellow, new Color32(255, 237, 40, 255));
            
            dicColor.Add(CurColor.Red | CurColor.White, new Color32(253, 168, 225, 255));
            dicColor.Add(CurColor.Blue | CurColor.White, new Color32(167, 140, 255, 255));
            dicColor.Add(CurColor.Yellow | CurColor.White, new Color32(255, 250, 208, 255));
            dicColor.Add(CurColor.Blue | CurColor.Yellow, new Color32(29, 118, 21, 255));
            dicColor.Add(CurColor.Red | CurColor.Blue, new Color32(186, 32, 255, 255));
            dicColor.Add(CurColor.Red | CurColor.Yellow, new Color32(255, 111, 0, 255));
            dicColor.Add(CurColor.Red | CurColor.Blue | CurColor.White, new Color32(226, 191, 255, 255));
            dicColor.Add(CurColor.Yellow | CurColor.Blue | CurColor.White, new Color32(209, 250, 150, 255));
            dicColor.Add(CurColor.Red | CurColor.Yellow | CurColor.White, new Color32(255, 205, 119, 255));
            dicColor.Add(CurColor.Red | CurColor.Blue | CurColor.Yellow, new Color32(67, 66, 63, 255));


            dicColor.Add(CurColor.Black, new Color32(67, 66, 63, 255));
        }

        public void AddDye(Dye dye)
        {
            curColor |= dye.color;
            UpdateColor();
        }

        public void UpdateColor()
        {
            cloth.color = dicColor[curColor];

            if (curColor == CurColor.Black)
                StartCoroutine(SetDefault());
        }

        public IEnumerator SetDefault()
        {
            yield return new WaitForSeconds(3);
            curColor = CurColor.None;
            cloth.color = dicColor[curColor];
        }

        public void CheckSuccess(CurColor color)
        {
            if (curColor == color)
            {
                // TODO : 미션 성공
            }
            else
            {
                // 미션 실패
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            ItemData item = eventData.pointerDrag.GetComponent<ItemData>();
            if (!(item.itemName == "WhiteDye" || item.itemName == "RedDye" ||
                item.itemName == "BlueDye" || item.itemName == "YellowDye"))
                return;

            Dye dye = item as Dye;
            AddDye(dye);              
        }
    }    
}
