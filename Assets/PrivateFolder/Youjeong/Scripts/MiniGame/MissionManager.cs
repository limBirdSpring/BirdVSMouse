using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;
using HyunJune;
using Photon.Pun;


namespace Youjeong
{
    public class MissionManager : MonoBehaviourPun
    {
        public PhotonView photon { get; private set; }

        [SerializeField]
        private CowManager cowManager;
        [SerializeField]
        private DyeManager dyeManager;
        [SerializeField]
        private HangariManager hangariManager;
        [SerializeField]
        private RopeManager ropeManager;

        [PunRPC]
        public void CowMissionUpdate(int birdCow, int mouseCow, bool[] birdActive, bool[] mouseActive)
        {
            cowManager.birdCowCount = birdCow;
            cowManager.mouseCowCount = mouseCow;
            cowManager.birdCowActive = birdActive;
            cowManager.mouseCowActive = mouseActive;
        }

        [PunRPC]
        public void ClothCurColorRPC(CurColor curColor)
        {
            dyeManager.cloth.curColor = curColor;
        }

        [PunRPC]
        public void HangariMissionUpdate(float water)
        {
            hangariManager.waterAmount = water;
        }

        [PunRPC]
        private void LoadUIRPC()
        {
            ropeManager.control.LoadUIRPC();
        }

        [PunRPC]
        private void SaveUIRPC(RopeGame[] ropes)
        {
            ropeManager.control.SaveUIRPC(ropes);
        }
    }
}

