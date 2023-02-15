using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using SoYoon;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;



namespace Saebom
{
    [Serializable]
    public enum Win
    {
        BirdWin,
        MouseWin,
        Draw
    };


    public class ScoreManager : SingleTon<ScoreManager>
    {

        //�����ջ� ����, �� ������ ���� �� ������ ��������, ��������� �� ����

        private int birdScore = 0;

        private int mouseScore = 0;

        private int birdCount;

        private int mouseCount;

        private bool isBirdSpyDie;

        private bool isMouseSpyDie;


        [SerializeField]
        private TextMeshProUGUI scoreUI;

        private PhotonView photonView;

        //============�¸�ȭ��===================

        [SerializeField]
        private GameObject canvas;

        [SerializeField]
        private Image birdSpyImg;

        [SerializeField]
        private Image mouseSpyImg;

        [SerializeField]
        private GameObject birdBackgroundImg;

        [SerializeField]
        private GameObject mouseBackgroundImg;

        [SerializeField]
        private GameObject drawBackgroundImg;

        [SerializeField]
        private GameObject birdHouseImg;

        [SerializeField]
        private GameObject mouseHouseImg;

        [SerializeField]
        private TextMeshProUGUI winText;

        [SerializeField]
        private GameObject exitButton;

        [SerializeField]
        private GameObject blockButton;

        [SerializeField]
        private GameObject map;

        //=======================================

        public int masterCheck = 0;

        private bool end;

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }
        private void OnEnable()
        {
            birdScore = mouseScore = 0;
            birdCount = mouseCount = PhotonNetwork.CountOfPlayers / 2 - 1;
            isBirdSpyDie = isMouseSpyDie = false;
        }


        //�� ���� ������ �ش� �Լ��� ȣ���ϸ� ������ �������
        public void CallScoreResultWindow()
        {
            //������ �ջ��ؼ� ��������
            StartCoroutine(CallScoreResultWindowCor());

        }
        private IEnumerator CallScoreResultWindowCor()
        {
            blockButton.SetActive(true);

            //�̼�â ���ֱ�
            MissionButton.Instance.MissionScreenOff();

            //���� ����
            map.SetActive(false);
            yield return new WaitForSeconds(2f);

            StartCoroutine(MissionButton.Instance.MissionCheckCor());

        }

        public IEnumerator ScoreResultCalculate()
        {
            yield return null;

            int score = MissionButton.Instance.MissionResultCheck();

            //���� ���
            if (!TimeManager.Instance.isCurNight)
                birdScore += score;
            else
                mouseScore += score;

            //��������� ���� �� �� ������ �����Ȳ ����
            if (PhotonNetwork.IsMasterClient)
                photonView.RPC("PrivatePlayerStateUpdate", RpcTarget.All, birdScore, mouseScore, birdCount, mouseCount, isBirdSpyDie, isMouseSpyDie);


            //���ھ� UI ����
            //���� ���� ���� ȿ�� �ִϸ��̼� �� ȿ���� �߰�
            if (score != 0)
                SoundManager.Instance.PlayUISound(UISFXName.ScoreUp);

            scoreUI.text = birdScore.ToString() + "   :   " + mouseScore.ToString();

            //���� ������ ��쿡�� ���� ���� ���
            if (TimeManager.Instance.isCurNight)
                TurnResult();

            Inventory.Instance.DeleteItem();//�κ��丮 ����

            //��ü���ֱ�
            GameObject[] corpse = GameObject.FindGameObjectsWithTag("Corpse");

            for (int i = 0; i < corpse.Length; i++)
            {
                Destroy(corpse[i]);
            }



            //�� 100%�ϰ�� 0%�� �ʱ�ȭ
            MissionButton.Instance.BakMissionReset();

            blockButton.SetActive(false);

            masterCheck = 0;
            //�÷��̾���� ��� ����Ȯ���� ���´��� Ȯ��

            if (end==false)
                photonView.RPC("PrivateScoreCheckFinish", RpcTarget.MasterClient, 1);

        }

        [PunRPC]
        public void PrivateScoreCheckFinish(int check)
        {
            masterCheck += check;

            Debug.Log(PhotonNetwork.PlayerList.Length);
            if (masterCheck == PhotonNetwork.PlayerList.Length) //����
            {
                TimeManager.Instance.FinishScoreTimeSet();
                masterCheck = 0;
            }
        }





        //�÷��̾ �׾��� �� ������ ��ǥ�� �÷��̾� ���¸� ������ 
        public void MasterCurPlayerStateUpdate()
        {
            //Ȱ���ð��� ��� ����Ǵ� ���

            //1. ��ǥ�� �ؼ� ���� ����� �������϶�

            //2. �����̰� �ù�1���� ����� ��� ����� �׿�����
            bool activeTimeOver = false;

            foreach (PlayerState state in PlayGameManager.Instance.playerList)
            {
                if (state.isBird)
                {
                    if (state.isDie && !state.isSpy)
                        birdCount--;
                    else if (state.isDie && state.isSpy)
                    {
                        isBirdSpyDie = true;
                        if (!TimeManager.Instance.isCurNight)
                            activeTimeOver = true;
                    }

                    if (birdCount == 1 && !TimeManager.Instance.isCurNight)
                        activeTimeOver = true;
                }
                else
                {
                    if (state.isDie && !state.isSpy)
                        mouseCount--;
                    else if (state.isDie && state.isSpy)
                    {
                        isMouseSpyDie = true;
                        if (TimeManager.Instance.isCurNight)
                            activeTimeOver = true;
                    }

                    if (mouseCount == 1 && TimeManager.Instance.isCurNight)
                        activeTimeOver = true;
                }
            }

            photonView.RPC("PrivatePlayerStateUpdate", RpcTarget.All, birdScore, mouseScore, birdCount, mouseCount, isBirdSpyDie, isMouseSpyDie);


            if (activeTimeOver)
                ActiveTimeOverNow();

        }

        //������ ���¸� ������ �� ������ �÷��̾�鿡�� ����
        [PunRPC]
        public void PrivatePlayerStateUpdate(int birdScore, int mouseScore, int birdCount, int mouseCount, bool isBirdSpyDie, bool isMouseSpyDie)
        {
            this.birdScore = birdScore;

            this.mouseScore = mouseScore;

            this.birdCount = birdCount;

            this.mouseCount = mouseCount;

            this.isBirdSpyDie = isBirdSpyDie;

            this.isMouseSpyDie = isMouseSpyDie;
        }


        //Ȱ���ð� ��� ����
        public void ActiveTimeOverNow()
        {

            //Ȱ���ð�������
            TimeManager.Instance.TimeOver();

        }


        //���� ������ �� ����� ���Դٸ� ���� �̰���� ����
        public void TurnResult()
        {
            //1. ������ ������ 6���� �Ѱ����� (������ �� ū ����� �̱�)
            if (birdScore >= 6 || mouseScore >= 6)
            {
                if (birdScore > mouseScore)
                    EndGame(Win.BirdWin);
                else if (birdScore < mouseScore)
                    EndGame(Win.MouseWin);
                else
                {
                    //���� �����̶�� �����̸� ���� ���� �̱��.
                    if (isBirdSpyDie && isMouseSpyDie)
                    {
                        //�Ѵ� �����̸� �׿��ٸ� ���� �ù��� ���� ���Ѵ�.
                        if (birdCount > mouseCount)
                        {
                            EndGame(Win.BirdWin);
                        }
                        else if (birdCount < mouseCount)
                        {
                            EndGame(Win.MouseWin);
                        }
                        else
                        {
                            EndGame(Win.Draw);
                        }
                    }
                    else
                    {
                        if (isBirdSpyDie)
                        {
                            EndGame(Win.MouseWin);
                        }
                        else if (isMouseSpyDie)
                        {
                            EndGame(Win.BirdWin);
                        }
                        else//�Ѵ� �����̸� ������ �ʾҴٸ� �ù��� ���� ���Ѵ�.
                        {
                            //�Ѵ� �����̸� �׿��ٸ� ���� �ù��� ���� ���Ѵ�.
                            if (birdCount > mouseCount)
                            {
                                EndGame(Win.BirdWin);
                            }
                            else if (birdCount < mouseCount)
                            {
                                EndGame(Win.MouseWin);
                            }
                            else
                            {
                                EndGame(Win.Draw);
                            }
                        }
                    }
                }
            }

            //2. ���� �����̰� �׾����� (���ʴ� �����̰� �׾��ٸ� ������ ��, �ƴ϶�� �����̸� �������� �̱�)
            else if (isBirdSpyDie || isMouseSpyDie)
            {
                if (isBirdSpyDie && isMouseSpyDie)
                {
                    //�Ѵ� �����̰� �׾��ٸ� ���� ������ ���Ѵ�.
                    if (birdScore > mouseScore)
                        EndGame(Win.BirdWin);
                    else if (birdScore < mouseScore)
                        EndGame(Win.MouseWin);
                    else
                    {
                        //������ �����̶�� ���� �ù��� ���� ���Ѵ�.
                        if (birdCount > mouseCount)
                        {
                            EndGame(Win.BirdWin);
                        }
                        else if (birdCount < mouseCount)
                        {
                            EndGame(Win.MouseWin);
                        }
                        else
                        {
                            EndGame(Win.Draw);
                        }

                    }

                }
                else if (isBirdSpyDie && !isMouseSpyDie)
                    EndGame(Win.BirdWin);
                else if (!isBirdSpyDie && isMouseSpyDie)
                    EndGame(Win.MouseWin);

            }
            //3. �������� ������1�� �ù�1���϶�
            else if ((birdCount <= 1 && !isBirdSpyDie) || (mouseCount <= 1 && !isMouseSpyDie))
            {
                //�Ѵ� ������1��, �ù�1���� ���Ҵٸ� ������ ���Ѵ�.
                if ((birdCount <= 1 && !isBirdSpyDie) && (mouseCount <= 1 && !isMouseSpyDie))
                {
                    if (birdScore > mouseScore)
                        EndGame(Win.BirdWin);
                    else if (birdScore < mouseScore)
                        EndGame(Win.MouseWin);
                    else
                        EndGame(Win.Draw);
                }
                else if ((birdCount <= 1 && !isBirdSpyDie) && !(mouseCount <= 1 && !isMouseSpyDie))
                    EndGame(Win.BirdWin);
                else if (!(birdCount <= 1 && !isBirdSpyDie) && (mouseCount <= 1 && !isMouseSpyDie))
                    EndGame(Win.MouseWin);
            }
            else if (TimeManager.Instance.curRound >= 11) //SettingManager.Instance.maxRoundCount
            {
                if (birdScore > mouseScore)
                    EndGame(Win.BirdWin);
                else if (birdScore < mouseScore)
                    EndGame(Win.MouseWin);
                else
                    EndGame(Win.Draw);
            }

            else//���и� �������� ��찡 �ƴϸ� ���� �簳
                TimeManager.Instance.FinishScoreTimeSet();


        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
                EndGame(Win.Draw);
            }
        }

        private void EndGame(Win win)
        {
            end = true;

            SoundManager.Instance.bgm.Stop();

            //���� �̰���� ��Ÿ���� ���� �·��� �ݿ���
            SoundManager.Instance.PlayUISound(UISFXName.Ending);
            //���� �Ǽ� +1

            DataManager.Instance.SaveResult(PlayResult.Play);

            if (PlayGameManager.Instance.myPlayerState.isSpy)
                DataManager.Instance.SaveResult(PlayResult.Spy);

            switch (win)
            {
                case Win.BirdWin:
                    //���� �¼� +1
                    if (PlayGameManager.Instance.myPlayerState.isBird == true && PlayGameManager.Instance.myPlayerState.isSpy == false ||
                        PlayGameManager.Instance.myPlayerState.isBird == false && PlayGameManager.Instance.myPlayerState.isSpy == true)
                        DataManager.Instance.SaveResult(PlayResult.Win);
                    else
                        DataManager.Instance.SaveResult(PlayResult.Lose);
                    //���� �м� +1
                    birdBackgroundImg.SetActive(true);
                    birdHouseImg.SetActive(true);
                    winText.text = "���� �¸�!";

                    break;
                case Win.MouseWin:
                    //���� �¼� +1
                    if (PlayGameManager.Instance.myPlayerState.isBird == false && PlayGameManager.Instance.myPlayerState.isSpy == false ||
                        PlayGameManager.Instance.myPlayerState.isBird == true && PlayGameManager.Instance.myPlayerState.isSpy == true)
                        DataManager.Instance.SaveResult(PlayResult.Win);
                    else
                        DataManager.Instance.SaveResult(PlayResult.Lose);
                    //���� �м� +1
                    mouseBackgroundImg.SetActive(true);
                    mouseHouseImg.SetActive(true);
                    winText.text = "���� �¸�!";

                    break;
                case Win.Draw:
                    //���� ���ºμ� +1
                    DataManager.Instance.SaveResult(PlayResult.Draw);
                    drawBackgroundImg.SetActive(true);
                    winText.text = "���º�!";

                    break;
            }

            //�̱� ��, ������ ��ü ���� �� ������ �̵�
            foreach (PlayerState state in PlayGameManager.Instance.playerList)
            {
                if (state.isBird && state.isSpy)
                    birdSpyImg.sprite = state.sprite;
                else if (!state.isBird && state.isSpy)
                    mouseSpyImg.sprite = state.sprite;
            }

            canvas.SetActive(true);

            //���� ���
            GetBadge();


            //������ ��ư ����
            StartCoroutine(EndCor());
        }

        private IEnumerator EndCor()
        {
            yield return new WaitForSeconds(3f);
            exitButton.SetActive(true);

        }

        private void GetBadge()
        {
            if (DataManager.Instance.myInfo.totalGame == 10)
                DataManager.Instance.EarnItemToMail("������ ���");
            else if (DataManager.Instance.myInfo.totalGame == 100)
                DataManager.Instance.EarnItemToMail("�������");
            else if (DataManager.Instance.myInfo.totalGame == 500)
                DataManager.Instance.EarnItemToMail("��������");

            if (DataManager.Instance.myInfo.win == 10)
                DataManager.Instance.EarnItemToMail("������ �¸�");
            else if (DataManager.Instance.myInfo.win == 100)
                DataManager.Instance.EarnItemToMail("���ν� �¸�");
            else if (DataManager.Instance.myInfo.win == 500)
                DataManager.Instance.EarnItemToMail("Ȳ�ݺ� �¸�");
        }




        public void OnExitButtonClick()
        {
            SceneManager.LoadScene("LobbyTestScene");
        }
    }
}
