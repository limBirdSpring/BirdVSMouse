using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoYoon
{
    [Serializable]
    public class MyInfo
    {
        public string lastChosenName = "";
        public int lastChosenBadge1 = -1;
        public int lastChosenBadge2 = -1;
        public int lastChosenCharacter = -1;

        public int totalGame = 0;
        public int win = 0;
        public int draw = 0;
        public int lose = 0;
        public int totalSpy = 0;

        public bool[] getPhoto;
        public bool[] getBadge;

        public MyInfo(int photoNum, int badgeNum)
        {
            getPhoto = new bool[photoNum];
            getBadge = new bool[badgeNum];
        }
    }
}
