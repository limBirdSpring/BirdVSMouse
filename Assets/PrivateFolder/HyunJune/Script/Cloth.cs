using Saebom;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;

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

    

    public class Cloth : Mission, IDropHandler
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
            cloth.color = dicColor[curColor];
        }

        public override void GraphicUpdate()
        {
            cloth.color = dicColor[curColor];
        }

        public override void PlayerUpdateCurMission()
        {
            photonView.RPC("ClothCurColorRPC", RpcTarget.All, curColor);
        }


        [PunRPC]
        public void ClothCurColorRPC(CurColor curColor)
        {
            this.curColor = curColor;
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

        public override bool GetScore()
        {
            // 지금이 밤 일떄
            if (TimeManager.Instance.isCurNight)
            {
                if (curColor == MissionButton.Instance.mouseMission.color)
                {
                    // 미션 성공
                    return true;
                }
                else
                {
                    // 미션 실패
                    return false;
                }
            }
            // 낮일 때
            else
            {
                if (curColor == MissionButton.Instance.birdMission.color)
                {
                    // 미션 성공
                    return true;
                }
                else
                {
                    // 미션 실패
                    return false;
                }
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
