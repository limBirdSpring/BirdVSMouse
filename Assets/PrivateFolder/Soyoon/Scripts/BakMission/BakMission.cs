using UnityEngine;

namespace SoYoon
{
    public class BakMission : MonoBehaviour
    {
        [SerializeField]
        private GameObject normalSaw;
        [SerializeField]
        private GameObject activeSaw;
        [SerializeField]
        private float onePersonBoostPercent;
        [SerializeField]
        private float twoPersonBoostPercent;
        [SerializeField]
        private float threePersonBoostPercent;
        [SerializeField]
        private float singleProgressPerDeltaTime;

        private float boostPercent;

        private void Start()
        {
            boostPercent = 1;
            normalSaw.SetActive(true);
            activeSaw.SetActive(false);
        }

        public void BeginSaw()
        {
            normalSaw.SetActive(false);
            activeSaw.SetActive(true);
        }
        
        public void EndSaw()
        {
            normalSaw.SetActive(true);
            activeSaw.SetActive(false);
        }

        public void UpdatePercentage()
        {
            int count = BakMissionManager.Instance.CurBakPlayerCount;
            Debug.Log(string.Format("현재 박타기를 플레이하고 있는 명 수 : {0}", count));

            if (count >= 1)
                boostPercent = onePersonBoostPercent;
            else if (count >= 2)
                boostPercent = twoPersonBoostPercent;
            else if (count >= 3)
                boostPercent = threePersonBoostPercent;
        }

        public float UpdateProgress()
        {
            Debug.Log(string.Format("개인 {0} * 부스터 {1} 의 속도로 박 자르는 중", singleProgressPerDeltaTime, boostPercent));
            return singleProgressPerDeltaTime * boostPercent * Time.deltaTime;
        }
    }
}
