using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saebom
{
    public class Mission : MonoBehaviourPunCallbacks
    {
        public virtual bool GetScore()
        {
            //������ ���� �� �ִ� ��Ȳ�̸� true, �ƴϸ� false
            return false;
        }

        public virtual void PlayerUpdateCurMission()
        {
            //�̼�â�� �ݴ� �� ���� ��ο��� �̼� �����Ȳ�� ������Ʈ����
            //�ڻ���
        }
    }

}