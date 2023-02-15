using Photon.Pun;
using Saebom;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SoYoon
{
    public class KillButton : MonoBehaviour
    {
        [HideInInspector]
        public GameObject target;
        [SerializeField]
        private Button killButton;

        private PlayerControllerTest controller;
        private GameObject killButtonGray;

        private void Awake()
        {
            Transform uiCanvas = GameObject.Find("UICanvas").transform;
            killButtonGray = uiCanvas.GetChild(10).gameObject;
        }

        private void Start()
        {
            controller = PlayGameManager.Instance.myPlayerState.playerPrefab.GetComponent<PlayerControllerTest>();
        }

        private void Update()
        {
            if (controller.CanKill)
            {
                killButton.interactable = true;
                Image thisImg = this.gameObject.GetComponent<Image>();
                Image childImg = this.transform.GetChild(0).GetComponent<Image>();
                thisImg.fillAmount = 1; thisImg.color = Color.white;
                childImg.fillAmount = 1; childImg.color = Color.white;
            }
            else
            {
                killButton.interactable = false;
                Image thisImg = this.gameObject.GetComponent<Image>();
                Image childImg = this.transform.GetChild(0).GetComponent<Image>();
                thisImg.fillAmount = controller.CurKillCoolTime / controller.KillCoolTime; thisImg.color = Color.gray;
                childImg.fillAmount = controller.CurKillCoolTime / controller.KillCoolTime; childImg.color = Color.gray;
            }
        }

        public void OnClickedKillButton()
        {
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
