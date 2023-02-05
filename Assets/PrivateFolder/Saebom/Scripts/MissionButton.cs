using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using HyunJune;
using System.Linq;
using TMPro;


//방장이 미션을 뽑음

//팀원의 팀에 맞게 전달 (텍스트 보여주는용)

//미션완료를 확인할때 방장에게 있는 저장정보로 미션완료를 판단함

//미션창들 출력해가면서 완료했는지 확인

//미션 완료시 방장의 scoremanager에게 정보 전달


namespace Saebom
{
    [Serializable]
    public struct MissionData
    {
       public CurColor color;
       public int water;
       public bool isBird;
    }


    public class MissionButton : SingleTon<MissionButton>
    {

        //==============미션을 저장하는 struct==================
        private Dictionary<int, CurColor> colorMissionData = new Dictionary<int, CurColor>();

        private MissionData birdMission;

        private MissionData mouseMission;

        public MissionData myMission;

        private PhotonView photonView;


        [SerializeField]
        private TextMeshProUGUI missionText;

        

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }

        private void Start()
        {
            birdMission.isBird = true;
            mouseMission.isBird = false;

            colorMissionData.Add(0, CurColor.Red | CurColor.White);
            colorMissionData.Add(1, CurColor.Blue | CurColor.White);
            colorMissionData.Add(2, CurColor.Yellow | CurColor.White);
            colorMissionData.Add(3, CurColor.Blue | CurColor.Yellow);
            colorMissionData.Add(4, CurColor.Red | CurColor.Blue);
            colorMissionData.Add(5, CurColor.Red | CurColor.Yellow);
            colorMissionData.Add(6, CurColor.Red | CurColor.Blue | CurColor.White);
            colorMissionData.Add(7, CurColor.Red | CurColor.Yellow | CurColor.White);
            colorMissionData.Add(8, CurColor.Yellow | CurColor.Blue | CurColor.White);
        }

        //플레이매니저에서 역할을 정한 다음 호출
        public void MissionShare()
        {
            if (PhotonNetwork.IsMasterClient)
                MasterGetMission();
            
        }

        //===============각 팀별 미션을 지정해주는 함수===========
        private void MasterGetMission()
        {
            birdMission.water = UnityEngine.Random.Range(7, 10) * 10;
            mouseMission.water = UnityEngine.Random.Range(7, 10) * 10;

            int birdColor = UnityEngine.Random.Range(0, colorMissionData.Count);
            int mouseColor = UnityEngine.Random.Range(0, colorMissionData.Count);

            photonView.RPC("UpdateMission", RpcTarget.All, birdColor, mouseColor, birdMission.water, mouseMission.water);
        }

        [PunRPC]
        public void UpdateMission(int birdColor, int mouseColor, int birdWater, int mouseWater)
        {
            //색상 넣기

            birdMission.color = colorMissionData[birdColor];
            mouseMission.color = colorMissionData[mouseColor];

            //물 넣기

            birdMission.water = birdWater;
            mouseMission.water = mouseWater;

            //내 미션 넣기
            if (PlayGameManager.Instance.myPlayerState.isBird == true)
            {
                myMission = birdMission;
            }
            else
            {
                myMission = mouseMission;
            }

            //미션UI에 내 미션 넣기

            string sunMoon = myMission.isBird ? "(해-해)" : "(달-달)";
            string team = myMission.isBird ? "새" : "쥐";
            string color = "";

            switch (myMission.color)
            {
                case CurColor.Red | CurColor.White :
                    color = "분홍색";
                    break;
                case CurColor.Blue | CurColor.White:
                    color = "하늘색";
                    break;
                case CurColor.Yellow | CurColor.White :
                    color = "연노랑색";
                    break;
                case CurColor.Blue | CurColor.Yellow:
                    color = "초록색";
                    break;
                case CurColor.Red | CurColor.Blue:
                    color = "보라색";
                    break;
                case CurColor.Red | CurColor.Yellow:
                    color = "주황색";
                    break;
                case CurColor.Red | CurColor.Blue | CurColor.White:
                    color = "연보라색";
                    break;
                case CurColor.Red | CurColor.Yellow | CurColor.White :
                    color = "오렌지색";
                    break;
                case  CurColor.Yellow | CurColor.Blue | CurColor.White :
                    color = "연두색";
                    break;
        }


            missionText.text =
                "미션1 : " + "박 잘라서 보물 획득하기" + "\n" +
                "미션2 : " + "동아줄 연결하기" + sunMoon + "\n" +
                "미션3 : " + "선녀옷 염색하기" + "(" + color + ")" + "\n" +
                "미션4 : " + "항아리에 물 채우기" + "(" + myMission.water.ToString() + "L)" + "\n" +
                "미션5 : " + team + "팀 거점에 소 가득 채우기";
        }


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