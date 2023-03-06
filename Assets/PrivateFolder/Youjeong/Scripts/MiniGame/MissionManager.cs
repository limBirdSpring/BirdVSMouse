using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;
using HyunJune;
using Photon.Pun;


namespace Youjeong
{
    [RequireComponent(typeof(PhotonView))]
    public class MissionManager : MonoBehaviourPunCallbacks
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

        private void Awake()
        {
            photon = GetComponent<PhotonView>();
        }

        [PunRPC]
        public void CowMissionCountUpdate(int birdCow, int mouseCow)
        {
            cowManager.birdCowCount = birdCow;
            cowManager.mouseCowCount = mouseCow;
        }

        [PunRPC]
        public void CowMissionActiveUpdate(bool[] birdActive, bool[] mouseActive)
        {
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
        public void SaveUIRPC(int[] ropes)
        {
            Debug.Log("SaveUIRPC");
            ropeManager.ropeGamesCurState = ropes;
        }
    }
}

