using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Saebom
{
    public class MissionManager : SingleTon<MissionManager>
    {

        //==============�̼��� �����ϴ� struct==================

        

        //===============�� ���� �̼��� �������ִ� �Լ�===========

        //������ �̼��� ����

        //������ ���� �°� ���� (�ؽ�Ʈ �����ִ¿�)

        //�̼ǿϷḦ Ȯ���Ҷ� ���忡�� �ִ� ���������� �̼ǿϷḦ �Ǵ���

        //�̼�â�� ����ذ��鼭 �Ϸ��ߴ��� Ȯ��

        //�̼� �Ϸ�� ������ scoremanager���� ���� ����



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