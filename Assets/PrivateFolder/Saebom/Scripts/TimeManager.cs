using Photon.Pun;
using SoYoon;
using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Saebom
{
    public class TimeManager : SingleTon<TimeManager>
    {
        //============수정불가 시간관련============

        public float curTime;

        //전체 시간
        private static float maxTime = 500f;


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

        [SerializeField]
        private TextMeshProUGUI hideranceUI;

        [SerializeField]
        private GameObject fastTime;


        //===================================

        [SerializeField]
        public bool timeOn { get; private set; } = false;

        [SerializeField]
        public bool isHouseTime { get; private set; } = false;

        [SerializeField]
        public bool isCurNight { get; private set; } = false;

        //=============================

        [HideInInspector]
        public float curRound = 0.5f;

        [SerializeField]
        private TextMeshProUGUI roundUI;

        //=============================

        [SerializeField]
        private GameObject endText;

        [SerializeField]
        private GameObject startText;


        private PhotonView photonView;


        public int hiderance=0;

        private bool isHidering = false;

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }

        private void OnEnable()
        {
            curRound = 0.5f;

            Hashtable props = new Hashtable();
            props.Add("curRound", curRound);
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }

        private void Start()
        {

            //전체 시간은 미리 설정된 데이타를 가져옴
            //maxTime = SettingManager.Instance.turnTime;

            halfTime = maxTime / 2;
            dangerTime = halfTime - 30f;
            dangerTime2 = maxTime - 30f;
        }


        private void Update()
        {
            //시간 강제로 증가 : 마지막에 삭제할 것
            if (Input.GetKeyDown(KeyCode.F2))
                AddTime(10);

            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (isHidering == false)
                    MasterTimeUpdate();
                else
                    MasterTimeUpdate(10f);
            }
            
        }

        // =================== 방장이 시간 더해줌 ========================

        private void MasterTimeUpdate(float sec = 1)
        {
            if (timeOn)
                curTime += Time.deltaTime * sec;

            photonView.RPC("PrivateTimeUpdate", RpcTarget.All, curTime);

            if (!timeOn)
                return;

            if (!isCurNight)
            {

                if (curTime > halfTime - 1 && curTime < halfTime)
                    photonView.RPC("TimeOver", RpcTarget.All);
                else if (curTime > dangerTime && curTime <= halfTime - 1)
                {
                    Debug.Log("방장 거점이동시간 시행");
                    photonView.RPC("DangerScreenOn", RpcTarget.All);
                }

            }
            //밤이면
            else
            {
                if (curTime > maxTime - 1 && curTime < maxTime)
                    photonView.RPC("TimeOver", RpcTarget.All);
                else if (curTime > dangerTime2 && curTime <= maxTime - 1)
                {
                    Debug.Log("방장 거점이동시간 시행 - 밤");
                    photonView.RPC("DangerScreenOn", RpcTarget.All);
                }
            }

           

        }

        [PunRPC]
        public void PrivateTimeUpdate(float masterCurTime)
        {
            curTime = masterCurTime;

            TimeSlideUpdate();

            if (curTime < halfTime)
                isCurNight = false;
            else if (curTime > halfTime)
                isCurNight = true;
        }

        //==========================시간 종료 후 점수합산==========================

        [PunRPC]
        public void TimeOver()
        { 
            if (!isCurNight && (curTime > halfTime - 1 && curTime < halfTime))
                isHouseTime = true;
            else if (isCurNight && (curTime > maxTime - 1 && curTime < maxTime))
                isHouseTime = true;

            TimeOff();

            PlayerControllerTest controller = PlayGameManager.Instance.myPlayerState.playerPrefab.GetComponent<PlayerControllerTest>();

            //만약 강제로 활동시간이 끝났다면 캐릭터 거점으로 강제이동
            //Debug.LogError("IsHouseTime : " + isHouseTime + " -> " + PlayGameManager.Instance.myPlayerState.playerPrefab.name);
            if (isHouseTime && ((controller.state == global::PlayerState.Active) || (controller.state == global::PlayerState.Inactive)))
                controller.gameObject.GetPhotonView().RPC("CheckIfIsInHouse", RpcTarget.All);

            PlayGameManager.Instance.PlayerGoHomeNow();

            if (PlayGameManager.Instance.myPlayerState.isSpy)
                controller.StopKillCoroutine();

            // 플레이어들 active, inactive 결정
            if ((int)curTime == (int)halfTime)
                controller.gameObject.GetPhotonView().RPC("SetActiveOrInactive", RpcTarget.All, true);
            else if ((int)curTime == (int)maxTime)
                controller.gameObject.GetPhotonView().RPC("SetActiveOrInactive", RpcTarget.All, false);

            hiderance = 0;
            hideranceUI.text = "방해성공 : " + hiderance.ToString() + "번";

            //2초 뒤 점수 확인 출력
            ScoreManager.Instance.CallScoreResultWindow();
        }

        private void TimeOff()
        {
            isHidering = false;
            
            timeOn = false;

            SoundManager.Instance.PlayUISound(UISFXName.Stop);
            //종료 텍스트 출력
            endText.SetActive(true);

            if (!isCurNight)
            {
                curTime = halfTime;
                nightSky.SetActive(true);
                SoundManager.Instance.bgm.clip = SoundManager.Instance.night;
                SoundManager.Instance.bgm.Play();
            }
            else
            {
                curTime = maxTime;
                nightSky.SetActive(false);
                SoundManager.Instance.bgm.clip = SoundManager.Instance.noon;
                SoundManager.Instance.bgm.Play();
            }
            redScreenUi.gameObject.SetActive(false);
            TimeSlideUpdate();
            FilterUpdate(0f);

        }

        //======================= 점수합산 끝나고 타임 On ========================

        public void FinishScoreTimeSet()
        {
            //점수확인 끝난 후 만약 밤이면 curTime = 0, TimeOn()
            if (isCurNight)
            {
                curTime = 0;
            }


            SetCurRound();

            photonView.RPC("TimeOn", RpcTarget.All);

        }

        [PunRPC]
        public void TimeOn()
        {

            isCurNight = !isCurNight;
            MissionButton.Instance.EmergencyUsed(MissionButton.Instance.birdEmergency, MissionButton.Instance.mouseEmergency);

            if (!PhotonNetwork.IsMasterClient)
                RoundSetting();
  
            isHouseTime = false;
            //시작 텍스트 출력
            SoundManager.Instance.PlayUISound(UISFXName.Start);
            startText.SetActive(true);



            PlayerControllerTest controller = PlayGameManager.Instance.myPlayerState.playerPrefab.GetComponent<PlayerControllerTest>();
            //Debug.LogError(controller.gameObject.name + " -> " + controller.gameObject.activeInHierarchy);
            if (PlayGameManager.Instance.myPlayerState.isSpy)
                controller.StartKillCoroutine();

            timeOn = true;
        
        }

        //==================== 시간관련 추가 함수들 ======================

        private void SetCurRound()
        {

            curRound +=0.5f;
            Debug.Log(curRound + "방장라운드");
            roundUI.text = "Round " + ((int)curRound).ToString();
            Hashtable props = new Hashtable();
            props.Add("curRound", curRound);
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);

        }

        private void RoundSetting()
        {
            Debug.Log(curRound + "팀원라운드");
            object curRoundObj;
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("curRound", out curRoundObj);
            curRound = (float)curRoundObj;
            roundUI.text = "Round " + ((int)curRound).ToString();

            Debug.Log(curRound + "출력된 팀원라운드");
        }


        public void TimeStop()
        {
            timeOn = false;
        }

        public void TimeResume()
        {
            timeOn = true;
        }

       

        public void AddTime(float sec)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                curTime += sec;
                photonView.RPC("PrivateTimeUpdate", RpcTarget.All, curTime);
            }
        }

      
        //================================== 필터 관련 ========================================
     

        //타임슬라이드 업데이트
        private void TimeSlideUpdate()
        {
            imgSlide.value = curTime / maxTime;
        }

        //거점이동시간 On
        [PunRPC]
        private void DangerScreenOn()
        {
            Debug.Log("거점이동시간 스크린과 필터 업데이트");

            redScreenUi.gameObject.SetActive(true);

            //필터 이미지 업데이트
            FilterUpdate(30);
        }

        //밤필터 일정시간동안 투명도 조정하기
        private void FilterUpdate(float sec)
        {

            if (!isCurNight)
            {
                Debug.Log("밤필터 시행");
                float alpha = (1 / sec * Time.deltaTime);
                nightFilter.color = new Color(255, 255, 255, Mathf.Clamp(nightFilter.color.a + alpha, 0, 1));
            }
            else
            {
                Debug.Log("낮필터 시행");
                float alpha = (1 / sec * Time.deltaTime);
                nightFilter.color = new Color(255, 255, 255, Mathf.Clamp(nightFilter.color.a - alpha, 0, 1));
            }
        }


        //============================방해관련==================================

        public void HideranceAdd()
        {
            photonView.RPC("HideranceAddToRPC", RpcTarget.All);
        }

        [PunRPC]
        private void HideranceAddToRPC()
        {
            hiderance++;
            hideranceUI.text = "방해성공 : " + hiderance.ToString() + "번";
            if (hiderance == 5) //SettingManager.Instance.successHiderance
            {
                isHidering = true;
                hiderance = 0;

                fastTime.SetActive(true);
                StartCoroutine(HideranceCor());
            }
        }

        private IEnumerator HideranceCor()
        {
            yield return new WaitForSeconds(5f);
            isHidering = false;
            fastTime.SetActive(false);
        }

    }
}