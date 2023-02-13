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
            Debug.Log("CowMissionCountUpdate");
            cowManager.birdCowCount = birdCow;
            cowManager.mouseCowCount = mouseCow;
            
        }

        [PunRPC]
        public void CowMissionActiveUpdate(bool[] birdActive, bool[] mouseActive)
        {
            Debug.Log("CowMissionActiveUpdate");
            
            cowManager.birdCowActive = birdActive;
            cowManager.mouseCowActive = mouseActive;
        }

        [PunRPC]
        public void ClothCurColorRPC(CurColor curColor)
        {
            Debug.Log("ClothCurColorRPC");
            dyeManager.cloth.curColor = curColor;
        }

        [PunRPC]
        public void HangariMissionUpdate(float water)
        {
            Debug.Log("HangariMissionUpdate");
            hangariManager.waterAmount = water;
        }

        [PunRPC]
        private void SaveUIRPC(int[] ropes)
        {
            Debug.Log("SaveUIRPC");
            ropeManager.ropeGamesCurState = ropes;
        }
    }
}

