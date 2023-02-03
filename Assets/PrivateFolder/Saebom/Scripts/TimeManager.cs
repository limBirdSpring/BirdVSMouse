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
        //��ü �ð�

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
            //���� �ջ�ð��� �ð� ���߱�

            //�����̵��ð��� ȿ�� �ֱ�
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
            //�ð��ʰ� �ؽ�Ʈ ���
        }
        
    }
}