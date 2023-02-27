using HyunJune;
using Photon.Pun;
using SoYoon;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//������ �̼��� ����

//������ ���� �°� ���� (�ؽ�Ʈ �����ִ¿�)

//�̼ǿϷḦ Ȯ���Ҷ� ���忡�� �ִ� ���������� �̼ǿϷḦ �Ǵ���

//�̼�â�� ����ذ��鼭 �Ϸ��ߴ��� Ȯ��

//�̼� �Ϸ�� ������ scoremanager���� ���� ����


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

        //==============�̼��� �����ϴ� struct==================
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
            Debug.Log(PlayGameManager.Instance.myPlayerState.name + birdEmergency + "(�� �̸�����)");
            Debug.Log(PlayGameManager.Instance.myPlayerState.name + birdEmergency + "(�� �̸�����)");
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

        //�÷��̸Ŵ������� ������ ���� ���� ȣ��
        public void MissionShare()
        {
            if (PhotonNetwork.IsMasterClient)
                MasterGetMission();
            
        }

        //===============�� ���� �̼��� �������ִ� �Լ�===========
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
            //���� �ֱ�

            birdMission.color = colorMissionData[birdColor];
            mouseMission.color = colorMissionData[mouseColor];

            //�� �ֱ�

            birdMission.water = birdWater;
            mouseMission.water = mouseWater;

            //�� �̼� �ֱ�
            if (PlayGameManager.Instance.myPlayerState.isBird == true)
            {
                myMission = birdMission;
            }
            else
            {
                myMission = mouseMission;
            }

            //�̼�UI�� �� �̼� �ֱ�

            string sunMoon = myMission.isBird ? "(��-��)" : "(��-��)";
            string team = myMission.isBird ? "��" : "��";
            string color = "";

            switch (myMission.color)
            {
                case CurColor.Red | CurColor.White :
                    color = "��ȫ��";
                    break;
                case CurColor.Blue | CurColor.White:
                    color = "�ϴû�";
                    break;
                case CurColor.Yellow | CurColor.White :
                    color = "�������";
                    break;
                case CurColor.Blue | CurColor.Yellow:
                    color = "�ʷϻ�";
                    break;
                case CurColor.Red | CurColor.Blue:
                    color = "�����";
                    break;
                case CurColor.Red | CurColor.Yellow:
                    color = "��Ȳ��";
                    break;
                case CurColor.Red | CurColor.Blue | CurColor.White:
                    color = "�������";
                    break;
                case CurColor.Red | CurColor.Yellow | CurColor.White :
                    color = "��������";
                    break;
                case  CurColor.Yellow | CurColor.Blue | CurColor.White :
                    color = "���λ�";
                    break;
        }


            missionText.text =
                "�̼�1 : " + "�� �߶� ���� ȹ���ϱ�" + "\n" +
                "�̼�2 : " + "������ �����ϱ�" + sunMoon + "\n" +
                "�̼�3 : " + "����� �����ϱ�" + "(" + color + ")" + "\n" +
                "�̼�4 : " + "�׾Ƹ��� �� ä���" + "(" + myMission.water.ToString() + "L)" + "\n" +
                "�̼�5 : " + team + "�� ������ �� ���� ä���";
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

            //��, ����, �÷��̼�
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

            //�� �̼�
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

            //�����̼�
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