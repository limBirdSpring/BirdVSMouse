using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Saebom;

namespace Youjeong
{
    public class CowHouse : MonoBehaviour, IDropHandler
    {
        private CowManager manager;

        [SerializeField]
        private bool isBrid;
        [SerializeField]
        private Button plusButton;

        [SerializeField]
        private AudioSource audioCowMoo;

        public bool isBirdHouse { get; private set; }
        public bool isClicked { get; private set; } = false;

        public Cow[] Cows;
        private bool isCow;

        private void Awake()
        {
            isBirdHouse = isBrid;
            manager = GetComponentInParent<CowManager>();
        }

        private void OnEnable()
        {
            if (isBirdHouse == TimeManager.Instance.isCurNight)
                plusButton.gameObject.SetActive(true);
            else 
                plusButton.gameObject.SetActive(false);
            manager.GetCowActive(isBirdHouse, Cows);
        }

        private void OnDisable()
        {
            if(isClicked && manager.GetCowCount(isBirdHouse)==1)
            {
                isClicked = false;
                manager.DeleteCow(isBirdHouse);
                foreach (Cow cow in Cows)
                    cow.gameObject.SetActive(false);
            }

            manager.SetCowsActive(isBirdHouse, Cows);
        }

        public void PlusCowButtonClick()
        {
            if (manager.GetCowCount(isBirdHouse) != 0 )
                return;
            if (Inventory.Instance.isItemSet("Cow"))
                return;
            
            isClicked = true;
            AudioPlay();
            OneMoreCow();
        }

        public void OneMoreCow()
        {
            for (int i = 0; i < Cows.Length; i++)
            {
                if (!Cows[i].gameObject.activeSelf)
                {
                    Cows[i].gameObject.SetActive(true);
                    manager.AddCow(isBirdHouse);
                    return;
                }
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (manager.GetCowCount(isBirdHouse) == 4)
                return;
            if (!(isBirdHouse == PlayGameManager.Instance.myPlayerState.isBird) && !PlayGameManager.Instance.myPlayerState.isSpy)
                return;

            isCow = Inventory.Instance.isItemSet("Cow");
            if (!isCow)
                return;
            Inventory.Instance.DeleteItem();
            AudioPlay();
            OneMoreCow();
        }

        private void AudioPlay()
        {
            audioCowMoo.Play();
        }
    }
}


