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
        //============수정불가 시간관련============

        public float curTime;

        //전체 시간
        private static float maxTime = 360f;


        private static float halfTime;

        private static float dangerTime;

        private static float dangerTime2;

        //===================================

        //==============이미지들==============

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
            //전체 시간은 미리 설정된 데이타를 가져옴
            //maxTime = SettingManager.Instance.turnTime;
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
                
                if (curTime > halfTime - 1 && curTime < halfTime)
                {
                    TimeOver();
                    isHouseTime = true;
                }
                else if (curTime > dangerTime && curTime < halfTime - 1)
                    DangerScreenOn();
            }
            else
            {
                if (curTime > maxTime - 1 && curTime < maxTime)
                {
                    TimeOver();
                    isHouseTime = true;
                }
                else if (curTime > dangerTime2 && curTime < maxTime - 1)
                    DangerScreenOn();
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


        public void TimeOn()
        {
            isHouseTime = false;
            //시작 텍스트 출력
            SoundManager.Instance.PlayUISound(UISFXName.Start);
            startText.SetActive(true);

            if (curTime == 0)
                SetCurRound();

            timeOn = true;
        }

        private void TimeOff()
        {

            timeOn = false;

            //종료 텍스트 출력
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

        public void TimeOver()
        {
            TimeOff();

            //만약에 살아있는 캐릭터중 거점 밖에 있는 캐릭터가 있으면 강제 사망함
            //만약 강제로 활동시간이 끝났다면 캐릭터 거점으로 강제이동
            if (!isHouseTime)
                PlayGameManager.Instance.PlayerGoHomeNow();
            else
            {
                PlayGameManager.Instance.myPlayerState.playerPrefab.GetComponent<PlayerControllerTest>(); //현재 거점에 있는지 확인 - 함수 추가
            }

            //2초 뒤 점수 확인 출력
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

        public void FinishScoreTimeSet()
        {
            //점수확인 끝난 후 만약 밤이면 curTime = 0, TimeOn()
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

            //필터 이미지 업데이트
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