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


        //점수합산 구현, 각 팀별로 점수 및 스파이 죽음여부, 남은사람들 수 저장

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
            //방장이 합산해서 공유해줌


            StartCoroutine(CallScoreResultWindowCor());



            
            
        }

        private IEnumerator CallScoreResultWindowCor()
        {
            yield return new WaitForSeconds(2f);

            //모시옷 점수 출력

            //항아리 점수 출력

            //외양간 점수 출력

            //동아줄 점수 출력

            //박 점수 출력

            yield return new WaitForSeconds(2f);



            //점수계산이 끝난 후 각 변수에 현재상황 저장
            if(PlayGameManager.Instance.myPlayerState.playerPrefab.GetComponent<PlayerController>().state == global::PlayerState.Ghost);


            //스코어 UI 변경
            //점수 갱신 위에 효과 애니메이션 및 효과음 추가

            scoreUI.text = birdScore.ToString() + "  :  " + mouseScore.ToString();

            //승패 여부 계산
            TurnResult();



        }

        private void MasterCurUpdate()
        {

        }


        public void ActiveTimeOverNow()//활동시간이 즉시 종료되는 경우
        {
            //1. 투표를 해서 잡은 사람이 스파이일때

            //2. 스파이가 시민1명을 남기고 모든 사람을 죽였을때

        }


        //턴이 끝났을 때 결과가 나왔다면 누가 이겼는지 구현
        public void TurnResult()
        {
            //1. 한쪽의 점수가 6점을 넘겼을때 (점수가 더 큰 사람이 이김)
            if (birdScore >= 6 || mouseScore >= 6)
            {
                if (birdScore > mouseScore)
                    EndGame(Win.BirdWin);
                else if (birdScore < mouseScore)
                    EndGame(Win.MouseWin);
                else
                {
                    //만약 동점이라면 스파이를 죽인 팀이 이긴다.
                    if (isBirdSpyDie && isMouseSpyDie)
                    {
                        //둘다 스파이를 죽였다면 남은 시민의 수로 비교한다.
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
                        else//둘다 스파이를 죽이지 않았다면 시민의 수로 비교한다.
                        {
                            //둘다 스파이를 죽였다면 남은 시민의 수로 비교한다.
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

            //2. 한쪽 스파이가 죽었을때 (양쪽다 스파이가 죽었다면 점수로 비교, 아니라면 스파이를 죽인팀이 이김)
            else if (isBirdSpyDie || isMouseSpyDie)
            {
                if (isBirdSpyDie && isMouseSpyDie)
                {
                    //둘다 스파이가 죽었다면 현재 점수로 비교한다.
                    if (birdScore >mouseScore)
                        EndGame(Win.BirdWin);
                    else if (birdScore < mouseScore)
                        EndGame(Win.MouseWin);
                    else
                    {
                        //점수가 동점이라면 남은 시민의 수로 비교한다.
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
            //3. 한쪽팀이 스파이1명 시민1명일때
            else if ((birdCount==1 && !isBirdSpyDie) || (mouseCount == 1 && !isMouseSpyDie))
            {
                //둘다 스파이1명, 시민1명이 남았다면 점수로 비교한다.
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
            else //승패를 결정짓는 경우가 아니면 게임 재개
                TimeManager.Instance.FinishScoreTimeSet();

            
        }

        public void EndGame(Win win)
        {
            //누가 이겼는지 나타내고 개인 승률에 반영함

            //개인 판수 +1

            switch(win)
            {
                case Win.BirdWin:
                    //개인 승수 +1
                    //개인 패수 +1
                    break;
                case Win.MouseWin:
                    //개인 승수 +1
                    //개인 패수 +1
                    break;
                case Win.Draw:
                    //개인 무승부수 +1
                    break;
            }

            //이긴 팀, 스파이 정체 공개 후 방으로 이동

            
        }


    }
}
