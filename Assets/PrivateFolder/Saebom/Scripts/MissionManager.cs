using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Saebom
{
    public class MissionManager : SingleTon<MissionManager>
    {

        //==============미션을 저장하는 struct==================

        

        //===============각 팀별 미션을 지정해주는 함수===========

        //방장이 미션을 뽑음

        //팀원의 팀에 맞게 전달 (텍스트 보여주는용)

        //미션완료를 확인할때 방장에게 있는 저장정보로 미션완료를 판단함

        //미션창들 출력해가면서 완료했는지 확인

        //미션 완료시 방장의 scoremanager에게 정보 전달



        //======================================================

        [SerializeField]
        private Button missionButton;


        [HideInInspector]
        public InterActionAdapter inter= null;


        public void MissionButtonOn()
        {
            if (inter !=null &&!inter.isActive)
            {
                missionButton.gameObject.SetActive(true);
                inter.OutLineOn();
            }
        }

        public void MissionButtonOff()
        {
            if (missionButton.gameObject.activeSelf == true)
            {
                inter.OutLineOff();
                inter = null;
                missionButton.gameObject.SetActive(false);
            }
        }

        public void OnMissionButtonClicked()
        {
            inter?.Interaction();
        }



        //========================================================

     

    }
}