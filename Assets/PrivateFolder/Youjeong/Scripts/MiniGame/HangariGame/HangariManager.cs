using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;

namespace Youjeong
{
    public class HangariManager : Mission
    {
        private HagnariGame game;
        private float waterAmount; 


        private void Awake()
        {
            game = GetComponent<HagnariGame>();
        }

        public override bool GetScore()
        {
            return true;
        }

        public override void GraphicUpdate()
        {

        }

        public override void PlayerUpdateCurMission()
        {
            waterAmount = game.amount;
        }
    }
}

