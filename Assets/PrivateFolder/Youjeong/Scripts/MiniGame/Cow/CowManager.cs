using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;
using Photon.Pun;

namespace Youjeong
{
    public class CowManager : Mission
    {
        [SerializeField]
        private PhotonView photon;

        public int birdCowCount = 2;
        public int mouseCowCount= 2;

        public bool[] birdCowActive = new bool[4];
        public bool[] mouseCowActive = new bool[4];

        private void Awake()
        {
            birdCowActive[0] = true;
            birdCowActive[1] = true;
            birdCowActive[2] = false;
            birdCowActive[3] = false;

            mouseCowActive[0] = true;
            mouseCowActive[1] = true;
            mouseCowActive[2] = false;
            mouseCowActive[3] = false;
        }

        public override void OnDisable()
        {
            PlayerUpdateCurMission();
            base.OnDisable();
        }

        public override void PlayerUpdateCurMission()
        {
            photon.RPC("CowMissionUpdate", RpcTarget.All, birdCowCount, mouseCowCount, birdCowActive, mouseCowActive);
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

