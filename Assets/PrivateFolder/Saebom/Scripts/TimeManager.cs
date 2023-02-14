using Photon.Pun;
using SoYoon;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;


namespace Saebom
{
    public class TimeManager : SingleTon<TimeManager>
    {
        //============�����Ұ� �ð�����============

        public float curTime;

        //��ü �ð�
        private static float maxTime = 360f;


        private static float halfTime;

        private static float dangerTime;

        private static float dangerTime2;

        //===================================

        //==============�̹�����==============

        [SerializeField]
        private Slider imgSlide;

        [SerializeField]
        private Image handle;

        [SerializeField]
        private Image redScreenUi;

        [SerializeField]
        private SpriteRenderer nightFilter;

        [SerializeField]
        private GameObject nightSky;


        //===================================


        private bool timeOn = false;

        public bool isHouseTime { get; private set; } = false;

        public bool isCurNight { get; private set; } = false;

        //=============================

        [HideInInspector]
        public int curRound = 0;

        [SerializeField]
        private TextMeshProUGUI roundUI;

        //=============================

        [SerializeField]
        private GameObject endText;

        [SerializeField]
        private GameObject startText;


        private PhotonView photonView;

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }

        private void Start()
        {
            //��ü �ð��� �̸� ������ ����Ÿ�� ������
            //maxTime = SettingManager.Instance.turnTime;
            halfTime = maxTime / 2;
            dangerTime = halfTime - 30f;
            dangerTime2 = maxTime - 30f;
        }


        private void Update()
        {
            //�ð� ������ ���� : �������� ������ ��
            if (Input.GetKeyDown(KeyCode.F2))
                AddTime(10);

            if (PhotonNetwork.IsMasterClient)
                MasterTimeUpdate();

            //���̸�
            
        }

        // =================== ������ �ð� ������ ========================

        private void MasterTimeUpdate()
        {
            if (timeOn)
                curTime += Time.deltaTime;

            photonView.RPC("PrivateTimeUpdate", RpcTarget.All, curTime);

            if (!isCurNight)
            {

                if (curTime > halfTime - 1 && curTime < halfTime)
                {
                    photonView.RPC("TimeOver", RpcTarget.All);
                    isHouseTime = true;
                }
                else if (curTime > dangerTime && curTime <= halfTime - 1)
                    DangerScreenOn();
            }
            //���̸�
            else
            {
                if (curTime > maxTime - 1 && curTime < maxTime)
                {
                    photonView.RPC("TimeOver", RpcTarget.All);
                    isHouseTime = true;
                }
                else if (curTime > dangerTime2 && curTime <= maxTime - 1)
                    DangerScreenOn();
            }

           

        }

        [PunRPC]
        public void PrivateTimeUpdate(float masterCurTime)
        {
            curTime = masterCurTime;

            TimeSlideUpdate();

            if (curTime <= halfTime)
                isCurNight = false;
            else
                isCurNight = true;
        }

        //==========================�ð� ���� �� �����ջ�==========================

        [PunRPC]
        public void TimeOver()
        {
            TimeOff();


            //���� ������ Ȱ���ð��� �����ٸ� ĳ���� �������� �����̵�
            if (!isHouseTime)
                PlayGameManager.Instance.PlayerGoHomeNow();
            else
            {
                PlayGameManager.Instance.myPlayerState.playerPrefab.GetComponent<PlayerControllerTest>(); //���� ������ �ִ��� Ȯ�� - �Լ� �߰�
            }

            //2�� �� ���� Ȯ�� ���
            ScoreManager.Instance.CallScoreResultWindow();


            for (int i = 0; i < PlayGameManager.Instance.playerList.Count; i++)
            {
                PlayerControllerTest controller = PlayGameManager.Instance.playerList[i].playerPrefab.GetComponent<PlayerControllerTest>();
                Debug.Log("player list count : " + PlayGameManager.Instance.playerList.Count);

                if (curTime == halfTime)
                {
                    controller.SetActiveOrInactive(true);
                }
                else if (curTime == maxTime)
                {
                    controller.SetActiveOrInactive(false);
                }
            }
        }

        private void TimeOff()
        {

            timeOn = false;

            //���� �ؽ�Ʈ ���
            endText.SetActive(true);

            if (!isCurNight)
            {
                curTime = halfTime;
                nightSky.SetActive(true);
                SoundManager.Instance.bgm.clip = SoundManager.Instance.night;
                SoundManager.Instance.bgm.Play();
            }
            else
            {
                curTime = maxTime;
                nightSky.SetActive(false);
                SoundManager.Instance.bgm.clip = SoundManager.Instance.noon;
                SoundManager.Instance.bgm.Play();
            }
            redScreenUi.gameObject.SetActive(false);
            TimeSlideUpdate();
            FilterUpdate(1f);

        }

        //======================= �����ջ� ������ Ÿ�� On========================

        public void FinishScoreTimeSet()
        {
            //����Ȯ�� ���� �� ���� ���̸� curTime = 0, TimeOn()
            if (isCurNight)
            {
                curTime = 0;
            }
            photonView.RPC("TimeOn", RpcTarget.All);
        }

        [PunRPC]
        public void TimeOn()
        {

            isHouseTime = false;
            //���� �ؽ�Ʈ ���
            SoundManager.Instance.PlayUISound(UISFXName.Start);
            startText.SetActive(true);

            if (curTime == 0)
                SetCurRound();

            MissionButton.Instance.MasterSetEmergency();

            timeOn = true;
        }

        //==================== �ð����� �߰� �Լ��� ======================

        private void SetCurRound()
        {
            curRound++;
            roundUI.text = "Round " + curRound.ToString();
        }

        public void TimeStop()
        {
            timeOn = false;
        }

        public void TimeResume()
        {
            timeOn = true;
        }

       

        public void AddTime(float sec)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                curTime += sec;
                photonView.RPC("PrivateTimeUpdate", RpcTarget.All, curTime);
            }
        }

      
        //================================== ���� ���� ========================================
     

        //Ÿ�ӽ����̵� ������Ʈ
        private void TimeSlideUpdate()
        {
            imgSlide.value = curTime / maxTime;
        }

        //�����̵��ð� On
        private void DangerScreenOn()
        {
            redScreenUi.gameObject.SetActive(true);

            //���� �̹��� ������Ʈ
            FilterUpdate(30);
        }

        //������ �����ð����� ���� �����ϱ�
        private void FilterUpdate(float sec)
        {

            if (!isCurNight)
            {
                float alpha = (1 / sec * Time.deltaTime);
                nightFilter.color = new Color(255, 255, 255, nightFilter.color.a + alpha);
            }
            else
            {
                float alpha = (1 / sec * Time.deltaTime);
                nightFilter.color = new Color(255, 255, 255, nightFilter.color.a - alpha);
            }
        }




    }
}