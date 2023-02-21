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

    

    public class Cloth : MonoBehaviour, IDropHandler
    {
        [HideInInspector]
        public Image clothImage;
        public Dictionary<CurColor, Color32> dicColor;

       
        //[HideInInspector]
        public CurColor curColor;

        [Header("Audio Source")]
        [SerializeField]
        private AudioSource DyeSound;

        [Header("Light Partucle")]
        [SerializeField]
        private LightParticle lightParticle;


        private void Awake()
        {
            clothImage = GetComponent<Image>();
            dicColor = new Dictionary<CurColor, Color32>();
            curColor = CurColor.None;
            Init();
            clothImage.color = dicColor[curColor];
        }

        private void OnEnable()
        {
            GraphicUpdate();
        }

        private void Init()
        {
            dicColor.Add(CurColor.None, new Color32(224, 216, 192, 255));

            dicColor.Add(CurColor.White, new Color32(255, 255, 255, 255));
            dicColor.Add(CurColor.Red, new Color32(255, 0, 0, 255));
            dicColor.Add(CurColor.Blue, new Color32(46, 86, 227, 255));
            dicColor.Add(CurColor.Yellow, new Color32(255, 237, 40, 255));
            
            dicColor.Add(CurColor.Red | CurColor.White, new Color32(253, 168, 225, 255));
            dicColor.Add(CurColor.Blue | CurColor.White, new Color32(107, 210, 250, 255));
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

        public void AddDye(CurColor color)
        {
            // 새로 넣는 염료를 이미 갖고 있다 -> 그럼 검정색으로 
            if (color == (curColor & color))
            {
                curColor |= CurColor.Black;
            }
            // 새로 넣는 염료가 없었다 그럼 넣는다
            else
            {
                curColor |= color;
            }

            UpdateColor();
        }

        public void GraphicUpdate()
        {
            Debug.Log(string.Format("Dye.GraphicUpdate : {0}", dicColor[curColor]));

            clothImage.color = new Color(dicColor[curColor].r / 255f, dicColor[curColor].g / 255f, dicColor[curColor].b / 255f);
            if (curColor == CurColor.Black)
            {
                curColor = CurColor.None;
                clothImage.color = dicColor[curColor];
            }
        }


        public void UpdateColor()
        {
            clothImage.color = dicColor[curColor];

            if (curColor == CurColor.Black)
                StartCoroutine(SetDefault());
        }

        public IEnumerator SetDefault()
        {
            yield return new WaitForSeconds(1f);
            curColor = CurColor.None;
            clothImage.color = dicColor[curColor];
        }

        public void OnDrop(PointerEventData eventData)
        {
            Inventory item = eventData.pointerDrag.GetComponent<Inventory>();
            if (Inventory.Instance.isItemSet("WhiteDye"))
            {
                AddDye(CurColor.White);
                PlayEffect();
                Inventory.Instance.DeleteItem();
            }
            else if (Inventory.Instance.isItemSet("RedDye"))
            {
                AddDye(CurColor.Red);
                PlayEffect();
                Inventory.Instance.DeleteItem();
            }
            else if (Inventory.Instance.isItemSet("YellowDye"))
            {
                AddDye(CurColor.Yellow);
                PlayEffect(); ;
                Inventory.Instance.DeleteItem();
            }
            else if (Inventory.Instance.isItemSet("BlueDye"))
            {
                AddDye(CurColor.Blue);
                PlayEffect();
                Inventory.Instance.DeleteItem();
            }
            else
                return;
        }

        private void PlayEffect()
        {
            DyeSound.Play();
            lightParticle.PlayParticle();
        }
    }    
}
