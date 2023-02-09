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
using SoYoon;


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


        [HideInInspector]
        public int mouseEmergency = 3;

        [HideInInspector]
        public int birdEmergency = 3;




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

            foreach (Mission mission in missionList)
            {
                mission.gameObject.SetActive(true);
                yield return new WaitForSeconds(1.5f);

                //���ø�Ʈ �ؽ�Ʈ ����
                yield return new WaitForSeconds(1f);

                mission.gameObject.SetActive(false);
                yield return new WaitForSeconds(1f);
            }

            StartCoroutine(ScoreManager.Instance.ScoreResultCalculate());

        }


        public void BakMissionReset()
        {
            missionList[0].gameObject.GetComponent<BakMissionManager>().BakMissionResetCalled();
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