using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Youjeong
{
    [RequireComponent(typeof(HangariManager))]
    public class HagnariGame : MonoBehaviour
    {
        private HangariManager hangariManager;
        public float amount = 0;
        public bool isOutHangari = false;

        [SerializeField]
        private InWater inWater;

        public void PlusWater()
        {
            amount += 10;
            inWater.FillWater(amount);
        }

        public void MinusWater()
        {
            if(amount==0)
                return;
            amount -= 10;
            inWater.FillWater(amount);
        }

        private void Awake()
        {
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

