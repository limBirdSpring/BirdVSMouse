using Photon.Pun;
using Photon.Realtime;
using Saebom;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SoYoon
{
    public class KillButton : MonoBehaviour
    {
        [SerializeField]
        private Button killButton;
        [HideInInspector]
        public GameObject target;

        private GameObject myPlayer;
        private PlayerControllerTest controller;

        private void Start()
        {
            myPlayer = PlayGameManager.Instance.myPlayerState.playerPrefab;
            controller = myPlayer.GetComponent<PlayerControllerTest>();
        }

        private void Update()
        {
            if (controller.CanKill)
            {
                Image thisImg = this.gameObject.GetComponent<Image>();
                Image childImg = this.transform.GetChild(0).GetComponent<Image>();
                thisImg.fillAmount = 1; thisImg.color = Color.white;
                childImg.fillAmount = 1; childImg.color = Color.white;
                killButton.interactable = true;
            }
            else
            {
                Image thisImg = this.gameObject.GetComponent<Image>();
                Image childImg = this.transform.GetChild(0).GetComponent<Image>();
                thisImg.fillAmount = controller.CurKillCoolTime / controller.KillCoolTime; thisImg.color = Color.gray;
                childImg.fillAmount = controller.CurKillCoolTime / controller.KillCoolTime; childImg.color = Color.gray;
                killButton.interactable = false;
            }
        }

        public void OnClickedKillButton()
        {
            if (PlayGameManager.Instance.myPlayerState.isSpy)
            {
                // 버튼을 누르는 순간에 타겟 설정
                Debug.LogError("Finding target");
                myPlayer.GetComponentInChildren<KillCollider>().FoundKillTarget();

                if (controller.TargetPlayerNum <= 0)
                    this.gameObject.SetActive(false);

                controller.StartKillCoroutine();
            }

            //target?.GetComponent<PlayerControllerTest>().Die();
            target?.GetPhotonView().RPC("Die", RpcTarget.All);
            // 죽은 상황에서는 kill 범위 감지 꺼주기
            target?.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
