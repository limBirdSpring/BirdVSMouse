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
            Debug.Log(string.Format("���� ��Ÿ�⸦ �÷����ϰ� �ִ� �� �� : {0}", count));

            if (count >= 1)
                boostPercent = onePersonBoostPercent;
            else if (count >= 2)
                boostPercent = twoPersonBoostPercent;
            else if (count >= 3)
                boostPercent = threePersonBoostPercent;
        }

        public float UpdateProgress()
        {
            Debug.Log(string.Format("���� {0} * �ν��� {1} �� �ӵ��� �� �ڸ��� ��", singleProgressPerDeltaTime, boostPercent));
            return singleProgressPerDeltaTime * boostPercent * Time.deltaTime;
        }
    }
}
