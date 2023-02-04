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
        [SerializeField]
        private List<Mission> missions;


        //�����ջ� ����, �� ������ ���� �� ������ ��������, ��������� �� ����

        private int birdScore;

        private int mouseScore;

        private int birdCount;

        private int mouseCount;

        private bool isBirdSpyDie;

        private bool isMouseSpyDie;


        [SerializeField]
        private TextMeshProUGUI scoreUI;


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



            //��������� ���� �� �� ������ �����Ȳ ����
            if(PlayGameManager.Instance.myPlayerState.playerPrefab.GetComponent<PlayerController>().state == global::PlayerState.Ghost);


            //���ھ� UI ����
            //���� ���� ���� ȿ�� �ִϸ��̼� �� ȿ���� �߰�

            scoreUI.text = birdScore.ToString() + "  :  " + mouseScore.ToString();

            //���� ���� ���
            TurnResult();



        }

        private void MasterCurUpdate()
        {

        }


        public void ActiveTimeOverNow()//Ȱ���ð��� ��� ����Ǵ� ���
        {
            //1. ��ǥ�� �ؼ� ���� ����� �������϶�

            //2. �����̰� �ù�1���� ����� ��� ����� �׿�����

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

        public void EndGame(Win win)
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
