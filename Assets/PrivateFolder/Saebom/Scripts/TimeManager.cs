using Photon.Pun;
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

        private float curTime;

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


        private bool timeOn = true;

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
            halfTime = maxTime / 2;
            dangerTime = halfTime - 30f;
            dangerTime2 = maxTime - 30f;
        }



        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.F2))
                AddTime(10);

            if (PhotonNetwork.IsMasterClient)
                MasterTimeUpdate();


            if (!isCurNight)
            {
                if (curTime > dangerTime && curTime < halfTime - 1)
                    DangerScreenOn();
                if (curTime > halfTime - 1 && curTime < halfTime)
                    TimeOver();
            }
            else
            {
                if (curTime > dangerTime2 && curTime < maxTime - 1)
                    DangerScreenOn();
                if (curTime > maxTime - 1 && curTime < maxTime)
                    TimeOver();
            }
        }

        public void TimeStop()
        {
            timeOn = false;
        }

        public void TimeResume()
        {
            timeOn = true;
        }

        private void MasterTimeUpdate()
        {
            if (timeOn)
                curTime += Time.deltaTime;

            photonView.RPC("PrivateTimeUpdate", RpcTarget.All, curTime);

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

        public void AddTime(float sec)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                curTime += sec;
                photonView.RPC("PrivateTimeUpdate", RpcTarget.All, curTime);
            }
        }

        private void SetCurRound()
        {
            curRound++;
            roundUI.text = "Round " + curRound.ToString();
        }


        private void TimeOn()
        {
            //���� �ؽ�Ʈ ���
            SoundManager.Instance.PlayUISound(UISFXName.Start);
            startText.SetActive(true);

            if (curTime == 0)
                SetCurRound();

            timeOn = true;
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
            }
            else
            {
                curTime = maxTime;
                nightSky.SetActive(false);
            }
            TimeSlideUpdate();
            FilterUpdate(1f);
            

        }

        public void TimeOver()
        {
            redScreenUi.gameObject.SetActive(false);
            TimeOff();

            //���࿡ ����ִ� ĳ������ ���� �ۿ� �ִ� ĳ���Ͱ� ������ ���� �����

            //2�� �� ���� Ȯ�� ���
            ScoreManager.Instance.CallScoreResultWindow();



        }

        public void FinishScoreTimeSet()
        {
            //����Ȯ�� ���� �� ���� ���̸� curTime = 0, TimeOn()
            if (isCurNight)
            {
                curTime = 0;
            }
            TimeOn();
        }


        private void TimeSlideUpdate()
        {
            imgSlide.value = curTime / maxTime;


        }

        private void DangerScreenOn()
        {
            redScreenUi.gameObject.SetActive(true);

            //���� �̹��� ������Ʈ
            FilterUpdate(30);
        }

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