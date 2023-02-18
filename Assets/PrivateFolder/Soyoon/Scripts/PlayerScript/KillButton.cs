using Photon.Pun;
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

        private GameObject myPlayer;
        private PlayerControllerTest controller;
        private GameObject target;

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
            // 버튼을 누르는 순간에 타겟 설정
            float killRangeRadius = myPlayer.transform.GetChild(1).GetComponent<CircleCollider2D>().radius;
            Collider2D[] targets = Physics2D.OverlapCircleAll(myPlayer.transform.position, killRangeRadius, LayerMask.NameToLayer("KillRange"));
            List<Collider2D> targetList = new List<Collider2D>();
            for(int i=0;i<targets.Length;i++)
            {
                Debug.Log("targets[i] : " + targets[i].transform.parent.name);
                if (targets[i] == myPlayer.transform.GetChild(1).GetComponent<Collider2D>())
                    continue;

                targetList.Add(targets[i]);
            }

            if(targetList.Count == 0)
            {
                this.gameObject.SetActive(false);
                return;
            }

            target = targetList[0].transform.parent.gameObject;
            float minDistance = (myPlayer.transform.position - target.transform.position).sqrMagnitude;
            for (int i = 0; i < targetList.Count; i++)
            {
                float distance = (myPlayer.transform.position - targetList[i].transform.position).sqrMagnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    target = targetList[i].transform.parent.gameObject;
                }
            }

            //target?.GetComponent<PlayerControllerTest>().Die();
            target?.GetPhotonView().RPC("Die", RpcTarget.All);
            // 죽은 상황에서는 kill 범위 감지 꺼주기
            target?.transform.GetChild(1).gameObject.SetActive(false);
            this.gameObject.SetActive(false);

            if (PlayGameManager.Instance.myPlayerState.isSpy)
                controller.StartKillCoroutine();
        }
    }
}
