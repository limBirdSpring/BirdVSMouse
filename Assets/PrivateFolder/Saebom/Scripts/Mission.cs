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
            //점수를 얻을 수 있는 상황이면 true, 아니면 false
            return false;
        }

        public virtual void GraphicUpdate()
        {
            //OnEnable()에서 호출
        }

        public virtual void PlayerUpdateCurMission()
        {
            //OnDisable()에서 호출
            //미션창을 닫는 그 순간 모두에게 미션 현재상황을 업데이트해줌
            //박빼고
        }
    }

}