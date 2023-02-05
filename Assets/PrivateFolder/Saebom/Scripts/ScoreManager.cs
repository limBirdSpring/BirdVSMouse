using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            yield return new WaitForSeconds(2f);

            //��ÿ� ���� ���

            //�׾Ƹ� ���� ���

            //�ܾ簣 ���� ���

            //������ ���� ���

            //�� ���� ���

            yield return new WaitForSeconds(2f);

            //���� ���

            //��������� ���� �� �� ������ �����Ȳ ����
            if (PhotonNetwork.IsMasterClient)
                photonView.RPC("PrivatePlayerStateUpdate", RpcTarget.All, birdScore, mouseScore, birdCount, mouseCount, isBirdSpyDie, isMouseSpyDie);

            //���ھ� UI ����
            //���� ���� ���� ȿ�� �ִϸ��̼� �� ȿ���� �߰�

            scoreUI.text = birdScore.ToString() + "  :  " + mouseScore.ToString();

            //���� ������ ��쿡�� ���� ���� ���
            if (TimeManager.Instance.isCurNight)
                TurnResult();



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

            //���� �Ǽ� +1

            switch(win)
            {
                case Win.BirdWin:
                    //���� �¼� +1
                    //���� �м� +1
                    break;
                case Win.MouseWin:
                    //���� �¼� +1
                    //���� �м� +1
                    break;
                case Win.Draw:
                    //���� ���ºμ� +1
                    break;
            }

            //�̱� ��, ������ ��ü ���� �� ������ �̵�

            
        }


    }
}
