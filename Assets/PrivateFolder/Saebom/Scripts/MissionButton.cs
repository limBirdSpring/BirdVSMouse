using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Saebom
{
    public class MissionButton : SingleTon<MissionButton>
    {

        [SerializeField]
        private Button missionButton;

        [SerializeField]
        private Button missionButtonGray;

        [HideInInspector]
        public InterActionAdapter inter;


        public void MissionButtonOn()
        {
            if (!inter.isActive)
                missionButton.gameObject.SetActive(true);
        }

        public void MissionButtonOff()
        {
            missionButton.gameObject.SetActive(false);
        }

        public void OnMissionButtonClicked()
        {
            inter?.Interaction();
        }


        //플레이어
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Saebom.MissionButton.Instance.inter = collision.GetComponent<InterActionAdapter>();
            Saebom.MissionButton.Instance.MissionButtonOn();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Saebom.MissionButton.Instance.inter = null;
            Saebom.MissionButton.Instance.MissionButtonOff();

        }

    }
}