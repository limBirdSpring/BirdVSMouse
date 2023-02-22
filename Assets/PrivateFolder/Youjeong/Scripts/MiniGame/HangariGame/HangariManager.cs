using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;

namespace Youjeong
{
    public class HangariManager : Mission
    {
        public float waterAmount=0;

        [SerializeField]
        private PhotonView photon;

        public void ResetGame()
        {
            waterAmount= 0;
        }

        public override bool GetScore()
        {
            if (!TimeManager.Instance.isCurNight && MissionButton.Instance.birdMission.water == waterAmount)
                return true;
            else if (TimeManager.Instance.isCurNight && MissionButton.Instance.mouseMission.water == waterAmount)
                return true;

            return false;

        }

        public override void OnDisable()
        {
            base.OnDisable();
            PlayerUpdateCurMission();
        }

        public override void GraphicUpdate()
        {
            
        }

        public override void PlayerUpdateCurMission()
        {
            photon.RPC("HangariMissionUpdate", RpcTarget.AllBuffered, waterAmount);
            
        }

    }
}

