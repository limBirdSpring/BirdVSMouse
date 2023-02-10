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
            // �������� Ȯ���� ��쿡�� ��� �÷��̾��� GetScore�Լ��� ȣ���� ��
            // �� ���̶� true�� ��ȯ�ϸ� �������� ó�� -> ����ȭ�Ǿ� �����Ƿ� ���� �� ȣ���
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
