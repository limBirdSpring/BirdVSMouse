using Photon.Pun;
using SoYoon;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Saebom
{
    public class TimeManager : SingleTon<TimeManager>
    {
        //============�����Ұ� �ð�����============

        public float curTime;

        //��ü �ð�
        private static float maxTime = 500f;


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

        [SerializeField]
        private GameObject nightSky;

        [SerializeField]
        private TextMeshProUGUI hideranceUI;


        //===================================


        public bool timeOn { get; private set; } = false;

        public bool isHouseTime { get; private set; } = false;

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

        private void Start()
        {
            curRound = 0.5f;

            Hashtable props = new Hashtable();
            props.Add("curRound", curRound);
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);

            //��ü �ð��� �̸� ������ ����Ÿ�� ������
            //maxTime = SettingManager.Instance.turnTime;

            halfTime = maxTime / 2;
            dangerTime = halfTime - 30f;
            dangerTime2 = maxTime - 30f;
        }


        private void Update()
        {
            //�ð� ������ ���� : �������� ������ ��
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

        // =================== ������ �ð� ������ ========================

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
                    photonView.RPC("DangerScreenOn", RpcTarget.All);

            }
            //���̸�
            else
            {
                if (curTime > maxTime - 1 && curTime < maxTime)
                    photonView.RPC("TimeOver", RpcTarget.All);
                else if (curTime > dangerTime2 && curTime <= maxTime - 1)
                    photonView.RPC("DangerScreenOn", RpcTarget.All);
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

        //==========================�ð� ���� �� �����ջ�==========================

        [PunRPC]
        public void TimeOver()
        { 
            if (!isCurNight && (curTime > halfTime - 1 && curTime < halfTime))
                isHouseTime = true;
            else if (isCurNight && (curTime > maxTime - 1 && curTime < maxTime))
                isHouseTime = true;

            TimeOff();

            PlayerControllerTest controller = PlayGameManager.Instance.myPlayerState.playerPrefab.GetComponent<PlayerControllerTest>();

            //���� ������ Ȱ���ð��� �����ٸ� ĳ���� �������� �����̵�
            if (isHouseTime && ((controller.state == global::PlayerState.Active) || (controller.state == global::PlayerState.Inactive)))
                controller.photonView.RPC("CheckIfIsInHouse", RpcTarget.All);

            PlayGameManager.Instance.PlayerGoHomeNow();

            if (PlayGameManager.Instance.myPlayerState.isSpy)
                controller.StopKillCoroutine();

            // �÷��̾�� active, inactive ����
            if ((int)curTime == (int)halfTime)
            {
                Debug.Log("�Լ� �ҷ���");
                controller.photonView.RPC("SetActiveOrInactive", RpcTarget.All, true);
            }
            else if ((int)curTime == (int)maxTime)
            {
                controller.photonView.RPC("SetActiveOrInactive", RpcTarget.All, false);
            }

            hiderance = 0;
            hideranceUI.text = "���ؼ��� : " + hiderance.ToString() + "��";

            //2�� �� ���� Ȯ�� ���
            ScoreManager.Instance.CallScoreResultWindow();
        }

        private void TimeOff()
        {
            isHidering = false;
            
            timeOn = false;

            SoundManager.Instance.PlayUISound(UISFXName.Stop);
            //���� �ؽ�Ʈ ���
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

        //======================= �����ջ� ������ Ÿ�� On ========================

        public void FinishScoreTimeSet()
        {
            //����Ȯ�� ���� �� ���� ���̸� curTime = 0, TimeOn()
            if (isCurNight)
            {
                curTime = 0;
            }

            SetCurRound();

            //�̸����� �ʱ�ȭ
            MissionButton.Instance.MasterSetEmergency();

            photonView.RPC("TimeOn", RpcTarget.All);
        }

        [PunRPC]
        public void TimeOn()
        {

            RoundSetting();
  
            isHouseTime = false;
            //���� �ؽ�Ʈ ���
            SoundManager.Instance.PlayUISound(UISFXName.Start);
            startText.SetActive(true);



            PlayerControllerTest controller = PlayGameManager.Instance.myPlayerState.playerPrefab.GetComponent<PlayerControllerTest>();
            if (PlayGameManager.Instance.myPlayerState.isSpy)
                controller.StartKillCoroutine();

            timeOn = true;
        }

        //==================== �ð����� �߰� �Լ��� ======================

        private void SetCurRound()
        {
            curRound +=0.5f;
            roundUI.text = "Round " + ((int)curRound).ToString();

            Hashtable props = new Hashtable();
            props.Add("curRound", curRound);
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }

        private void RoundSetting()
        {
            object curRoundObj;
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("curRound", out curRoundObj);
            curRound = (float)curRoundObj;
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

      
        //================================== ���� ���� ========================================
     

        //Ÿ�ӽ����̵� ������Ʈ
        private void TimeSlideUpdate()
        {
            imgSlide.value = curTime / maxTime;
        }

        //�����̵��ð� On
        [PunRPC]
        private void DangerScreenOn()
        {
            redScreenUi.gameObject.SetActive(true);

            //���� �̹��� ������Ʈ
            FilterUpdate(30);
        }

        //������ �����ð����� ���� �����ϱ�
        private void FilterUpdate(float sec)
        {

            if (!isCurNight)
            {
                float alpha = (1 / sec * Time.deltaTime);
                nightFilter.color = new Color(255, 255, 255, nightFilter.color.a + alpha);
            }
            else
            {
                float alpha = (1 / sec * Time.deltaTime);
                nightFilter.color = new Color(255, 255, 255, nightFilter.color.a - alpha);
            }
        }


        //============================���ذ���==================================

        public void HideranceAdd()
        {
            photonView.RPC("HideranceAddToRPC", RpcTarget.All);
        }

        [PunRPC]
        private void HideranceAddToRPC()
        {
            hiderance++;
            hideranceUI.text = "���ؼ��� : " + hiderance.ToString() + "��";
            if (hiderance == 5) //SettingManager.Instance.successHiderance
            {
                isHidering = true;
                hiderance = 0;

                StartCoroutine(HideranceCor());
            }
        }

        private IEnumerator HideranceCor()
        {
            yield return new WaitForSeconds(5f);
            isHidering = false;
        }

    }
}