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
        private float maxTime = 180f;//3��
        //��ü �ð�

        [SerializeField]
        private Image imgSlide;

        [SerializeField]
        private Image handle;


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

            TimeUpdate();

            if (curTime > dangerTime)
                redScreenUi.gameObject.SetActive(true);
            if (curTime > maxTime)
                TimeOver();
        }

        private void TimeUpdate()
        {
            if (curTime < maxTime)
                curTime += Time.deltaTime;
        }

        private void TimeOver()
        {
            //�ð��ʰ� �ؽ�Ʈ ���
        }
        
    }
}