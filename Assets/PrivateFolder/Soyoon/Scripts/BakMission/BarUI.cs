using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoYoon
{
    public class BarUI : MonoBehaviour
    {
        [SerializeField]
        private Image whiteBar;
        [SerializeField]
        private Image redBar;
        [SerializeField]
        private Transform miniBak;
        [SerializeField]
        private Transform bakStartPos;
        [SerializeField]
        private Transform bakEndPos;

        private float distanceDiff;
        private float prevBakProgress;

        private void Start()
        {
            distanceDiff = bakEndPos.position.x - bakStartPos.position.x;
            prevBakProgress = 0;
            if (BakMissionManager.Instance.CurBakPlayerCount <= 1)
            {
                whiteBar.color = Color.white;
                redBar.color = Color.clear;
            }
            else
            {
                whiteBar.color = Color.clear;
                redBar.color = Color.white;
            }
            whiteBar.fillAmount = 0;
            redBar.fillAmount = 0;
            miniBak.position = bakStartPos.position;
        }

        public void ChangedPlayerCount()
        {
            if (BakMissionManager.Instance.CurBakPlayerCount <= 1)
            {
                whiteBar.color = Color.white;
                redBar.color = Color.clear;
            }
            else
            {
                whiteBar.color = Color.clear;
                redBar.color = Color.white;
            }
        }

        private void Update()
        {
            whiteBar.fillAmount = BakMissionManager.Instance.CurBakProgress * 0.01f;
            redBar.fillAmount = BakMissionManager.Instance.CurBakProgress * 0.01f;
            float progressToMiniBakPos = (BakMissionManager.Instance.CurBakProgress * 0.01f) * distanceDiff;
            miniBak.Translate(progressToMiniBakPos - prevBakProgress, 0, 0);
            prevBakProgress = progressToMiniBakPos;
        }

        public void BakMissionReset()
        {
            prevBakProgress = 0;
            if (BakMissionManager.Instance.CurBakPlayerCount <= 1)
            {
                whiteBar.color = Color.white;
                redBar.color = Color.clear;
            }
            else
            {
                whiteBar.color = Color.clear;
                redBar.color = Color.white;
            }
            whiteBar.fillAmount = 0;
            redBar.fillAmount = 0;
            miniBak.position = bakStartPos.position;
        }
    }
}
