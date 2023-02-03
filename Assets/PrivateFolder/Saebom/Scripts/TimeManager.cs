using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;


namespace Saebom
{
    public class TimeManager : MonoBehaviour
    {
        private float curTime;

        [SerializeField]
        private float maxTime = 10000f;
        //전체 시간

        [SerializeField]
        private Slider imgSlide;

        [SerializeField]
        private Image handle;

        public bool timeAdd = true;


        [SerializeField]
        private Image redScreenUi;


        private float dangerTime;

        private void Start()
        {
            dangerTime = maxTime - 30f;
        }


        private void Update()
        {
            //점수 합산시간에 시간 멈추기

            //거점이동시간에 효과 넣기
            if (timeAdd)
                TimeUpdate();

            if (curTime > dangerTime)
                redScreenUi.gameObject.SetActive(true);
            if (curTime > maxTime)
                TimeOver();
        }

        private void TimeUpdate()
        {
            curTime += Time.deltaTime;

            imgSlide.value = curTime/maxTime * 100;
        }

        private void TimeOver()
        {
            //시간초과 텍스트 출력
        }
        
    }
}