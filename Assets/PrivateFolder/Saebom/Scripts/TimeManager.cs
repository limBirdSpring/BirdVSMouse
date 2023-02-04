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

        private float curTime;

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

        public bool isCurNight { get; private set; } = false;

        //=============================

        [HideInInspector]
        public int curRound = 0;

        [SerializeField]
        private TextMeshProUGUI roundUI;



        private void Start()
        {
            halfTime = maxTime / 2;
            dangerTime = halfTime - 30f;
            dangerTime2 = maxTime - 30f;
        }



        private void Update()
        {
            //점수 합산시간에 시간 멈추기 - 플레이매니저에서 조정

            if (Input.GetKeyDown(KeyCode.F2))
                AddTime(10);

            if (timeOn)
                TimeUpdate();

            if (!isCurNight)
            {
                if (curTime > dangerTime)
                    DangerScreenOn();
                if (curTime > halfTime - 1)
                    TimeOver();
            }
            else
            {
                if (curTime > dangerTime2)
                    DangerScreenOn();
                if (curTime > maxTime - 1)
                    TimeOver();
            }
        }

        private void TimeUpdate()
        {
            curTime += Time.deltaTime;
            TimeSlideUpdate();

            if (curTime < halfTime)
                isCurNight = false;
            else
                isCurNight = true;
        }

        public void AddTime(float sec)
        {
            curTime += sec;
        }

        private void SetCurRound()
        {
            curRound++;
            roundUI.text = "Round " + curRound.ToString();
        }


        private void TimeOn()
        {
            //시작 텍스트 출력
            if (curTime == 0)
                SetCurRound();

            timeOn = true;
        }

        private void TimeOff()
        {

            timeOn = false;

            //종료 텍스트 출력


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

        private void TimeOver()
        {
            TimeOff();

            redScreenUi.gameObject.SetActive(false);

            //시간초과 텍스트 출력


            //2초 뒤 점수 확인 출력


  
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