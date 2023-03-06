using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoYoon
{
    public class BakUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject openedBak;
        [SerializeField]
        private GameObject closedBak;

        private void Start()
        {
            openedBak.SetActive(false);
            closedBak.SetActive(true);
        }

        public void BakMissionComplete()
        {
            openedBak.SetActive(true);
            closedBak.SetActive(false);
        }

        public void BakMissionReset()
        {
            openedBak.SetActive(false);
            closedBak.SetActive(true);
        }
    }
}
