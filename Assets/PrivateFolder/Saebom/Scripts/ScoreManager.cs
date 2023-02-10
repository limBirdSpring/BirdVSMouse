using Photon.Pun;
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

        private int birdScore=0;

        private int mouseScore=0;

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

        //=======================================

        private int masterCheck = 0;

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

            scoreUI.text = birdScore.ToString() + "  :  " + mouseScore.ToString();

            //���� ������ ��쿡�� ���� ���� ���
            if (TimeManager.Instance.isCurNight)
                TurnResult();

            Inventory.Instance.DeleteItem();//�κ��丮 ����

            //��ü���ֱ�
            GameObject[] corpse = GameObject.FindGameObjectsWithTag("Corpse");

            for (int i=0; i<corpse.Length;i++)
            {
                Destroy(corpse[i]);
            }

            //�� 100%�ϰ�� 0%�� �ʱ�ȭ

            MissionButton.Instance.BakMissionReset();

            blockButton.SetActive(false);

            photonView.RPC("PrivateScoreCheckFinish", RpcTarget.MasterClient, 1);

        }

        [PunRPC]
        public void PrivateScoreCheckFinish(int check)
        {
            masterCheck += check;

            if (masterCheck == PhotonNetwork.PlayerList.Length)
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
            //��� Ȱ���ð��� ���� ��쿡�� �������� �̵����� �ʾƵ� ���� �ʰ�, �ڵ����� �������� �̵��ǵ��� ����


            //�÷��̾ ��� �������� �����̵�


            //Ȱ���ð�������
            TimeManager.Instance.TimeOver();

        }


        //���� ������ �� ����� ���Դٸ� ���� �̰���� ����
        private void TurnResult()
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
                    if (birdScore >mouseScore)
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
            else if ((birdCount==1 && !isBirdSpyDie) || (mouseCount == 1 && !isMouseSpyDie))
            {
                //�Ѵ� ������1��, �ù�1���� ���Ҵٸ� ������ ���Ѵ�.
                if ((birdCount == 1 && !isBirdSpyDie) && (mouseCount == 1 && !isMouseSpyDie))
                {
                    if (birdScore > mouseScore)
                        EndGame(Win.BirdWin);
                    else if (birdScore < mouseScore)
                        EndGame(Win.MouseWin);
                    else
                        EndGame(Win.Draw);
                }
                else if ((birdCount == 1 && !isBirdSpyDie) && !(mouseCount == 1 && !isMouseSpyDie))
                    EndGame(Win.BirdWin);
                else if (!(birdCount == 1 && !isBirdSpyDie) && (mouseCount == 1 && !isMouseSpyDie))
                    EndGame(Win.MouseWin);
            }
            else //���и� �������� ��찡 �ƴϸ� ���� �簳
                TimeManager.Instance.FinishScoreTimeSet();

            
        }

        private void EndGame(Win win)
        {
            //���� �̰���� ��Ÿ���� ���� �·��� �ݿ���
            SoundManager.Instance.PlayUISound(UISFXName.Ending);
            //���� �Ǽ� +1

            switch (win)
            {
                case Win.BirdWin:
                    //���� �¼� +1
                    //���� �м� +1
                    birdBackgroundImg.SetActive(true);
                    birdHouseImg.SetActive(true);
                    winText.text = "���� �¸�!";

                    break;
                case Win.MouseWin:
                    //���� �¼� +1
                    //���� �м� +1
                    mouseBackgroundImg.SetActive(true);
                    mouseHouseImg.SetActive(true);
                    winText.text = "���� �¸�!";

                    break;
                case Win.Draw:
                    //���� ���ºμ� +1
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


            //������ ��ư ����
            StartCoroutine(EndCor());
        }

        private IEnumerator EndCor()
        {
            yield return new WaitForSeconds(3f);
            exitButton.SetActive(true);

        }


    }
}
