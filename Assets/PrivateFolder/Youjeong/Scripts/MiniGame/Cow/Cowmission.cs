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
            if (!TimeManager.Instance.isCurNight && isBirdHouse)
            {
                if (manager.birdCowCount == 4)
                    return true;
            }
            else if (!TimeManager.Instance.isCurNight && !isBirdHouse)
                return false;
            else if (TimeManager.Instance.isCurNight && !isBirdHouse)
            {
                if (manager.mouseCowCount == 4)
                    return true;
            }
            else if (TimeManager.Instance.isCurNight && isBirdHouse)
                return false;

            return false;
        }
    }
}


