using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;


namespace Saebom
{
    public class TimeManager : MonoBehaviour
    {
        //============�����Ұ� �ð�����============

        private float curTime;

        [SerializeField]
        private static float maxTime = 360f;
        //��ü �ð�

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

        //===================================


        private bool timeOn = true;

        public bool isCurNight { get; private set; } = false;







        private void Start()
        {
            dangerTime = halfTime - 30f;
            dangerTime2 = maxTime - 30f;
            halfTime = maxTime / 2;
        }

        public void TimeOn()
        {
            //���� �ؽ�Ʈ ���
            timeOn = true;
        }

        public void TimeOff()
        {
            //���� �ؽ�Ʈ ���
            timeOn = false;

            if (!isCurNight)
            {
                curTime = halfTime;
            }
            else
            {
                curTime = maxTime;
            }
            TimeSlideUpdate();
        }


        private void Update()
        {
            //���� �ջ�ð��� �ð� ���߱� - �÷��̸Ŵ������� ����

            if (timeOn)
                TimeUpdate();

            if (!isCurNight)
            {
                if (curTime > dangerTime)
                    DangerScreenOn();
                if (curTime > halfTime-1)
                    TimeOver();
            }
            else
            {
                if (curTime > dangerTime2)
                    DangerScreenOn();
                if (curTime > maxTime-1)
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
                isCurNight=true;   
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
            //����ð��� ���� ȭ��� �ٲ�� �����ʿ�

            if (!isCurNight)
            {
                float alpha = 1f / sec * Time.deltaTime;
                nightFilter.color = new Color(255, 255, 255, nightFilter.color.a + alpha);
            }
            else
            {
                float alpha = 1f / sec * Time.deltaTime;
                nightFilter.color = new Color(255, 255, 255, nightFilter.color.a - alpha);
            }
        }


        private void TimeOver()
        {
            timeOn = false;
            redScreenUi.gameObject.SetActive(false);
            //�ð��ʰ� �ؽ�Ʈ ���


            //2�� �� ���� Ȯ�� ���


            //����Ȯ�� ���� �� TimeOn
        }
        
    }
}