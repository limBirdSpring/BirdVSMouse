using HyunJune;
using Photon.Pun;
using SoYoon;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


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

        [HideInInspector]
        public MissionData birdMission;

        [HideInInspector]
        public MissionData mouseMission;

        public MissionData myMission;

        private PhotonView photonView;


        [SerializeField]
        private TextMeshProUGUI missionText;

        [SerializeField]
        private List<Mission> missionList;

        [SerializeField]
        private RopeController ropeCheck;


        [SerializeField]
        private GameObject completeText;


        public int mouseEmergency = 3;
        public int birdEmergency = 3;

        [SerializeField]
        private TextMeshProUGUI emergencyUI;



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

            birdEmergency = mouseEmergency = 3;
            //birdEmergency = mouseEmergency = SettingManager.Instance.emergencyCount;
        }

        public void MasterSetEmergency()
        {
            Debug.Log(PlayGameManager.Instance.myPlayerState.name + birdEmergency + "(새 이머젼시)");
            Debug.Log(PlayGameManager.Instance.myPlayerState.name + birdEmergency + "(쥐 이머젼시)");
            photonView.RPC("SetEmergencyCountUI", RpcTarget.All, !TimeManager.Instance.isCurNight ? birdEmergency : mouseEmergency);
        }

        [PunRPC]
        private void SetEmergencyCountUI(int emergency)
        {

            if (!TimeManager.Instance.isCurNight)
            {
                birdEmergency = emergency;
                emergencyUI.text = birdEmergency.ToString();
                Debug.Log(PlayGameManager.Instance.myPlayerState.name + emergency);
            }
            else
            {
                mouseEmergency = emergency;
                emergencyUI.text = mouseEmergency.ToString();
                Debug.Log(PlayGameManager.Instance.myPlayerState.name + emergency);
            }
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
        

        public int MissionResultCheck()
        {
            int score=0;

            foreach (Mission mission in missionList)
            {
                if (mission.GetScore())
                    score++;
            }

            

            return score;
        }

        public IEnumerator MissionCheckCor()
        {
            yield return new WaitForSeconds(1f);

            //박, 콩쥐, 컬러미션
            for (int i = 0; i < 3; i++)
            {
                missionList[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(1.5f);
                if (missionList[i].GetScore())
                {
                    SoundManager.Instance.PlayUISound(UISFXName.MissionComplete);
                    completeText.SetActive(true);
                }
                yield return new WaitForSeconds(1f);

                missionList[i].gameObject.SetActive(false);
                yield return new WaitForSeconds(1f);
            }

            //소 미션
            missionList[3].gameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            if (missionList[3].GetScore()&&!TimeManager.Instance.isCurNight)
            {
                SoundManager.Instance.PlayUISound(UISFXName.MissionComplete);
                completeText.SetActive(true);
            }
            yield return new WaitForSeconds(1f);

            missionList[3].gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            //==============================================
            missionList[4].gameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            if (missionList[4].GetScore() && TimeManager.Instance.isCurNight)
            {
                SoundManager.Instance.PlayUISound(UISFXName.MissionComplete);
                completeText.SetActive(true);
            }
            yield return new WaitForSeconds(1f);

            missionList[4].gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);

            //로프미션
            missionList[5].gameObject.SetActive(true);

            ropeCheck.SunOrMoonStart();

            while (!ropeCheck.isArrive)
            {
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            if (missionList[5].GetScore())
            {
                SoundManager.Instance.PlayUISound(UISFXName.MissionComplete);
                completeText.SetActive(true);
            }

            ropeCheck.SunOrMoonReset();

            missionList[5].gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);

            ScoreManager.Instance.ScoreResultCalculate();

        }


        public void BakMissionReset()
        {
            if (missionList[0].GetScore())
                BakMissionManager.Instance.BakMissionResetCalled();
            //missionList[0].gameObject.GetComponent<BakMissionManager>().BakMissionResetCalled();
        }

        public void MissionScreenOff()
        {
            foreach (Mission mission in missionList)
            {
                mission.gameObject.SetActive(false);
            }
        }


        public void Emergency()
        {
            if (PlayGameManager.Instance.myPlayerState.isBird)
            {
                photonView.RPC("EmergencyUsed", RpcTarget.All, birdEmergency-1, mouseEmergency);
            }
            else
            {
                photonView.RPC("EmergencyUsed", RpcTarget.All, birdEmergency, mouseEmergency-1);
            }
        }

        [PunRPC]
        private void EmergencyUsed(int birdE, int mouseE)
        {
            birdEmergency = birdE;
            mouseEmergency = mouseE;
        }




        //======================================================

        [SerializeField]
        private Button missionButton;


        [HideInInspector]
        public InterActionAdapter inter= null;

        public void MissionButtonOn()
        {
            missionButton.gameObject.SetActive(true);
            //if (inter !=null &&!inter.isActive)
            //{
            //    missionButton.gameObject.SetActive(true);
            //    inter?.OutLineOn();
            //}
            //else if (inter != null)
            //{
            //    missionButton.gameObject.SetActive(false);
            //    inter?.OutLineOff();
            //}
        }

        public void MissionButtonOff()
        {
            inter = null;
            missionButton.gameObject.SetActive(false);
            //if (missionButton.gameObject.activeSelf == true)
            //{
            //    inter?.OutLineOff();
            //    inter = null;
            //    missionButton.gameObject.SetActive(false);
            //}
        }

        public void OnMissionButtonClicked()
        {
            PlayerControllerTest myPlayer = PlayGameManager.Instance.myPlayerState.playerPrefab.GetComponent<PlayerControllerTest>();

            InterActionAdapter targetAdapter = null;
            float minDistance = float.MaxValue;
            foreach(InterActionAdapter adapter in myPlayer.Interactions)
            {
                if (adapter.isActive) continue;

                float distance = (myPlayer.transform.position - adapter.gameObject.transform.position).sqrMagnitude;
                if(distance < minDistance) 
                { 
                    targetAdapter = adapter;
                    minDistance = distance;
                }
            }

            inter = targetAdapter;
            inter?.Interaction();
        }



        //========================================================

     

    }
}