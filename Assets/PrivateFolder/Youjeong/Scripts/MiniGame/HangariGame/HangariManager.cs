using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;

namespace Youjeong
{
    [RequireComponent(typeof(PhotonView))]
    public class HangariManager : Mission
    {
        private PhotonView photon;

        public float waterAmount=0;
    
        private void Awake()
        {
            photon = GetComponent<PhotonView>();
        }

        public void ResetGame()
        {
            waterAmount= 0;
        }

        public override bool GetScore()
        {
           /* if (!TimeManager.Instance.isCurNight && MissionButton.Instance.birdMission.water == waterAmount)
                return true;
            else if (TimeManager.Instance.isCurNight && MissionButton.Instance.mouseMission.water == waterAmount)
                return true;*/

            return false;

        }

        public override void GraphicUpdate()
        {
            
        }

        public override void PlayerUpdateCurMission()
        {
            photon.RPC("MissionUpdate", RpcTarget.All, waterAmount);
            
        }

        [PunRPC]
        public void MissionUpdate(float water)
        {
            waterAmount = water;
        }
    }
}

