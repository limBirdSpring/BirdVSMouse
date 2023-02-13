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
        public string lastChosenBadge1 = "";
        public string lastChosenBadge2 = "";
        public string lastChosenCharacter = "";

        public int totalGame = 0;
        public int win = 0;
        public int draw = 0;
        public int lose = 0;
        public int totalSpy = 0;

        public List<string> earnedItem;
        public List<string> mailedItem;
    }
}
