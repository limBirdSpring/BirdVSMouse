using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Youjeong
{
    [RequireComponent(typeof(HangariManager))]
    public class HagnariGame : MonoBehaviour
    {
        [SerializeField]
        private InWater inWater;

        public float delay { get; private set; } = 3f;

        private HangariManager hangariManager;

        public void PlusWater()
        {
            hangariManager.waterAmount += 20;
            inWater.ChangeWater(hangariManager.waterAmount);

        }

        public void MinusWater()
        {
            if(hangariManager.waterAmount == 0)
                return;
            hangariManager.waterAmount -= 10;
            inWater.ChangeWater(hangariManager.waterAmount);
        }

        private void Awake()
        {
            //GetComponent<AudioSource>().Play();
            hangariManager = GetComponent<HangariManager>();
        }

        private void OnEnable()
        {
            hangariManager.GraphicUpdate();
        }

        private void OnDisable()
        {
            hangariManager.PlayerUpdateCurMission();
        }

    }
}

