using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;

namespace Youjeong
{
    public class Cowmission : Mission
    {
        [SerializeField]
        private CowManager manager;
        [SerializeField]
        private bool isBirdHouse;

        public override bool GetScore()
        {
            if (!TimeManager.Instance.isCurNight && manager.birdCowCount == 4 && isBirdHouse)
                return true;
            else if (TimeManager.Instance.isCurNight && manager.mouseCowCount == 4 && isBirdHouse)
                return true;

            return false;
        }
    }
}


