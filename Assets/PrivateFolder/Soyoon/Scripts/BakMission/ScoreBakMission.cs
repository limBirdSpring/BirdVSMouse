using Saebom;
using UnityEngine;

namespace SoYoon
{
    public class ScoreBakMission : Mission
    {
        [SerializeField]
        private BakMissionManager bakManager;

        public override bool GetScore()
        {
            // 최종으로 확인할 경우에는 모든 플레이어의 GetScore함수를 호출한 뒤
            // 한 명이라도 true를 반환하면 성공으로 처리 -> 동기화되어 있으므로 같은 값 호출됨
            if (bakManager.CurBakProgress >= 100)
                return true;
            else
                return false;
        }

        public override void GraphicUpdate()
        {
            // !Do Nothing
        }

        public override void PlayerUpdateCurMission()
        {
            // !Do Nothing
        }
    }
}
