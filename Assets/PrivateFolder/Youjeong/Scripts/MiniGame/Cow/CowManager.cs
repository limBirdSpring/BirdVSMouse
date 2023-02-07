using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;
using Photon.Pun;

namespace Youjeong
{
    [RequireComponent(typeof(PhotonView))]
    public class CowManager : Mission
    {
        private PhotonView photon;

        private int birdCowCount=2;
        private int mouseCowCount=2;

        private bool[] birdCowActive = new bool[4];
        private bool[] mouseCowActive = new bool[4];

        private void Awake()
        {
            photon = GetComponent<PhotonView>();
            birdCowActive[0] = true;
            birdCowActive[1] = true;
            mouseCowActive[0] = true;
            mouseCowActive[1] = true;
        }

        public override bool GetScore()
        {
            if (!TimeManager.Instance.isCurNight && birdCowCount==4)
                return true;
            else if (TimeManager.Instance.isCurNight && mouseCowCount==4)
                return true;

            return false;
        }

        public override void PlayerUpdateCurMission()
        {
            photon.RPC("CowMissionUpdate", RpcTarget.All, birdCowCount, mouseCowCount, birdCowActive, mouseCowActive);

        }

        [PunRPC]
        public void CowMissionUpdate(int birdCow,int mouseCow, bool[] birdActive, bool[] mouseActive)
        {
            birdCowCount = birdCow;
            mouseCowCount = mouseCow;
            birdCowActive = birdActive;
            mouseCowActive = mouseActive;
        }

        public void AddCow(bool isbirdHouse)
        {
            if (isbirdHouse)
                birdCowCount++;
            else
                mouseCowCount++;
        }

        public void DeleteCow(bool isbirdHouse)
        {
            if (isbirdHouse)
                birdCowCount--;
            else
                mouseCowCount--;
        }

        public int GetCowCount(bool isbirdHouse)
        {
            if (isbirdHouse)
                return birdCowCount;
            else
                return mouseCowCount;
        }

        public void SetCowsActive(bool isbirdHouse, Cow[] cows)
        {
            if (isbirdHouse)
                for(int i=0; i<4;i++)
                {
                    birdCowActive[i] = cows[i].gameObject.activeSelf;
                }
            else
                for (int i = 0; i < 4; i++)
                {
                    mouseCowActive[i] = cows[i].gameObject.activeSelf;
                }
        }
        public void GetCowActive(bool isbirdHouse, Cow[] cows)
        {
            if (isbirdHouse)
                for (int i = 0; i < 4; i++)
                {
                    cows[i].gameObject.SetActive(birdCowActive[i]);
                }
            else
                for (int i = 0; i < 4; i++)
                {
                    cows[i].gameObject.SetActive(mouseCowActive[i]);
                }
        }
    }
}

