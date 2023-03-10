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
using Hashtable = ExitGames.Client.Photon.Hashtable;



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

        //점수합산 구현, 각 팀별로 점수 및 스파이 죽음여부, 남은사람들 수 저장

        private int birdScore = 0;

        private int mouseScore = 0;

        private int birdCount;

        private int mouseCount;

        private bool isBirdSpyDie;

        private bool isMouseSpyDie;


        private int allPlayerNum;


        [SerializeField]
        private TextMeshProUGUI scoreUI;

        private PhotonView photonView;

        //============승리화면===================

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
            allPlayerNum = birdCount = mouseCount = PhotonNetwork.CurrentRoom.PlayerCount / 2 - 1;
            isBirdSpyDie = isMouseSpyDie = false;

            Hashtable hashtable = new Hashtable() {
                { "MousePoint", 0 },
                { "BirdPoint", 0 }
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }


        //각 턴이 끝나고 해당 함수를 호출하면 점수를 출력해줌
        public void CallScoreResultWindow()
        {
            StartCoroutine(CallScoreResultWindowCor());

        }
        private IEnumerator CallScoreResultWindowCor()
        {
            blockButton.SetActive(true);

            //미션창 없애기
            MissionButton.Instance.MissionScreenOff();

            //지도 끄기
            map.SetActive(false);
            yield return new WaitForSeconds(2f);


            StartCoroutine(MissionButton.Instance.MissionCheckCor());

        }

        public  void ScoreResultCalculate()
        {
            masterCheck = 0;
            photonView.RPC("PrivateScoreCheckFinish", RpcTarget.MasterClient, 1);
        }

        [PunRPC]
        public void PrivateScoreCheckFinish(int check)
        {
            masterCheck += check;

            Debug.Log(PhotonNetwork.PlayerList.Length);
            if (masterCheck == PhotonNetwork.PlayerList.Length) //수정
            {
                int score = MissionButton.Instance.MissionResultCheck();

                //점수 계산
                if (!TimeManager.Instance.isCurNight)
                    birdScore += score;
                else
                    mouseScore += score;

                //점수계산이 끝난 후 각 변수에 현재상황 저장
                photonView.RPC("PrivatePlayerStateUpdate", RpcTarget.Others, birdScore, mouseScore, birdCount, mouseCount, isBirdSpyDie, isMouseSpyDie);

                //각자 점수 UI변경 후 맵 초기화
                photonView.RPC("ScoreUpdate", RpcTarget.All, score);

                //턴이 끝났을 경우에만 승패 여부 계산
                if (TimeManager.Instance.isCurNight)
                    photonView.RPC("TurnResult", RpcTarget.MasterClient);
                else
                    TimeManager.Instance.FinishScoreTimeSet();
                
            }
        }


        [PunRPC]
        public void ScoreUpdate(int score)
        { 
            //스코어 UI 변경
            //점수 갱신 위에 효과 애니메이션 및 효과음 추가
            if (score != 0)
                SoundManager.Instance.PlayUISound(UISFXName.ScoreUp);

            scoreUI.text = birdScore.ToString() + "   :   " + mouseScore.ToString();

            ////턴이 끝났을 경우에만 승패 여부 계산
            //if (TimeManager.Instance.isCurNight)
            //    TurnResult();

            Inventory.Instance.DeleteItem();//인벤토리 비우기

            //시체없애기
            GameObject[] corpse = GameObject.FindGameObjectsWithTag("Corpse");

            for (int i = 0; i < corpse.Length; i++)
            {
                Destroy(corpse[i]);
            }



            //박 100%일경우 0%로 초기화
            MissionButton.Instance.BakMissionReset();

            blockButton.SetActive(false);

            masterCheck = 0;         

        }






        //플레이어가 죽었을 때 방장이 대표로 플레이어 상태를 갱신함 
        public void MasterCurPlayerStateUpdate()
        {
            //활동시간이 즉시 종료되는 경우

            //1. 투표를 해서 잡은 사람이 스파이일때

            //2. 스파이가 시민1명을 남기고 모든 사람을 죽였을때
            bool activeTimeOver = false;

            birdCount = allPlayerNum;
            mouseCount = allPlayerNum;
            isMouseSpyDie = false;
            isBirdSpyDie = false;

            Debug.Log("새팀 전체 시민 수 :" + birdCount);
            Debug.Log("쥐팀 전체 시민 수 :" + mouseCount);

            foreach (PlayerState state in PlayGameManager.Instance.playerList)
            {
                Debug.Log("이름 : " + state.name + ", 새 : " + state.isBird + ", 죽은여부 : " + state.isDie + ", 스파이여부 : " + state.isSpy);

                if (state.isBird)
                {
                    if (state.isDie && !state.isSpy)
                    {
                        birdCount--;
                    }
                        
                    else if (state.isDie && state.isSpy)
                    {
                        isBirdSpyDie = true;
                        if (!TimeManager.Instance.isCurNight)
                        {
                            Debug.Log("승패조건 1");
                            activeTimeOver = true;
                            
                        }
                    }

                    if (birdCount <= 1 && !TimeManager.Instance.isCurNight)
                    {
                         activeTimeOver = true;
                         Debug.Log("승패조건 2"); 
                    }
                }
                else
                {
                    if (state.isDie && !state.isSpy)
                        mouseCount--;
                    else if (state.isDie && state.isSpy)
                    {
                        isMouseSpyDie = true;
                        if (TimeManager.Instance.isCurNight)
                        {
                            Debug.Log("승패조건 3");
                            activeTimeOver = true;
                            
                        }
                    }

                    if (mouseCount <= 1 && TimeManager.Instance.isCurNight)
                    {
                        Debug.Log("승패조건 4");
                        activeTimeOver = true;
                       
                    }
                }
            }
            Debug.Log("새팀점수 : " + birdScore + " 쥐팀점수 : " + mouseScore + "남은새팀수 : " + birdCount + "남은쥐팀수 : " + mouseCount + "새팀스파이 죽음여부 :" + isBirdSpyDie + "쥐팀스파이 죽음여부 : " + isMouseSpyDie);

            photonView.RPC("PrivatePlayerStateUpdate", RpcTarget.All, birdScore, mouseScore, birdCount, mouseCount, isBirdSpyDie, isMouseSpyDie);


            if (activeTimeOver)
                ActiveTimeOverNow();

        }

        //방장이 상태를 갱신한 후 나머지 플레이어들에게 전달
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


        //활동시간 즉시 종료
        public void ActiveTimeOverNow()
        {

            //활동시간끝내기
            TimeManager.Instance.gameObject.GetPhotonView().RPC("TimeOver", RpcTarget.All);
            //TimeManager.Instance.TimeOver();
        }


        [PunRPC]
        //턴이 끝났을 때 결과가 나왔다면 누가 이겼는지 구현
        public void TurnResult()
        {

             Win whoWin = new Win();

            //1. 한쪽의 점수가 6점을 넘겼을때 (점수가 더 큰 사람이 이김)
            if (birdScore >= 6 || mouseScore >= 6)
            {
                Debug.Log("한쪽의 점수가 6점을 넘겼을 때");
                if (birdScore > mouseScore)
                {
                    whoWin = Win.BirdWin;
                }
                else if (birdScore < mouseScore)
                {
                    whoWin = (Win.MouseWin);
                }
                else
                {
                    Debug.Log("한쪽의 점수가 6점을 넘겼을 때 : 동점일떄");
                    //만약 동점이라면 스파이를 죽인 팀이 이긴다.
                    if (isBirdSpyDie && isMouseSpyDie)
                    {
                        Debug.Log("한쪽의 점수가 6점을 넘겼을 때 : 두 스파이 모두 죽었을 때");
                        //둘다 스파이를 죽였다면 남은 시민의 수로 비교한다.
                        if (birdCount > mouseCount)
                        {
                            Debug.Log("한쪽의 점수가 6점을 넘겼을 때 : 두 스파이 모두 죽었을 때 : 새팀시민 많음");
                            whoWin = (Win.BirdWin);
                        }
                        else if (birdCount < mouseCount)
                        {
                            Debug.Log("한쪽의 점수가 6점을 넘겼을 때 : 두 스파이 모두 죽었을 때 : 쥐팀시민 많음");
                            whoWin = (Win.MouseWin);
                        }
                        else
                        {
                            whoWin = (Win.Draw);
                        }
                    }
                    else
                    {
                        
                        if (isBirdSpyDie)
                        {
                            Debug.Log("한쪽의 점수가 6점을 넘겼을 때 : 새스파이가 죽었을떄");
                            whoWin = (Win.BirdWin);
                        }
                        else if (isMouseSpyDie)
                        {
                            Debug.Log("한쪽의 점수가 6점을 넘겼을 때 : 쥐스파이가 죽었을떄");
                            whoWin = (Win.MouseWin);
                        }
                        else//둘다 스파이를 죽이지 않았다면 시민의 수로 비교한다.
                        {
                            //둘다 스파이를 죽였다면 남은 시민의 수로 비교한다.
                            if (birdCount > mouseCount)
                            {
                                Debug.Log("한쪽의 점수가 6점을 넘겼을 때 : 새 시민이 쥐 시민보다 많이 남았을때");
                                whoWin = (Win.BirdWin);
                            }
                            else if (birdCount < mouseCount)
                            {
                                Debug.Log("한쪽의 점수가 6점을 넘겼을 때 : 쥐 시민이 새 시민보다 많이 남았을때");
                                whoWin = (Win.MouseWin);
                            }
                            else
                            {
                                whoWin = (Win.Draw);
                            }
                        }
                    }
                }
            }
            /*
            //2. 한쪽 스파이가 죽었을때 (양쪽다 스파이가 죽었다면 점수로 비교, 아니라면 스파이를 죽인팀이 이김)
            else if (isBirdSpyDie || isMouseSpyDie)
            {
                Debug.Log("한쪽 스파이가 죽었을때");
                if (isBirdSpyDie && isMouseSpyDie)
                {
                    Debug.Log("한쪽 스파이가 죽었을때 : 둘다 죽었을때");
                    //둘다 스파이가 죽었다면 현재 점수로 비교한다.
                    if (birdScore > mouseScore)
                    {
                        Debug.Log("한쪽 스파이가 죽었을때 : 새팀 점수가 더 높을때");
                        whoWin = (Win.BirdWin);
                    }
                    else if (birdScore < mouseScore)
                    {
                        Debug.Log("한쪽 스파이가 죽었을때 : 쥐팀 점수가 더 높을때");
                        whoWin = (Win.MouseWin);
                    }
                    else
                    {
                        Debug.Log("한쪽 스파이가 죽었을때 : 점수가 동점일때");
                        //점수가 동점이라면 남은 시민의 수로 비교한다.
                        if (birdCount > mouseCount)
                        {
                            Debug.Log("한쪽 스파이가 죽었을때 : 새팀이 더 많이 남았을때");
                            whoWin = (Win.BirdWin);
                        }
                        else if (birdCount < mouseCount)
                        {
                            Debug.Log("한쪽 스파이가 죽었을때 : 쥐팀이 더 많이 남았을떄");
                            whoWin = (Win.MouseWin);
                        }
                        else
                        {
                            whoWin = (Win.Draw);
                        }

                    }

                }
                else if (isBirdSpyDie && !isMouseSpyDie)
                {
                    Debug.Log("한쪽 스파이가 죽었을때 : 새스파이가 죽음");
                    whoWin = (Win.BirdWin);
                }
                else if (!isBirdSpyDie && isMouseSpyDie)
                {
                    Debug.Log("한쪽 스파이가 죽었을때 : 쥐스파이가 죽음");
                    whoWin = (Win.MouseWin);
                }

            }
            */
            //3. 한쪽팀이 스파이1명 시민1명일때
            else if ((birdCount <= 1 && !isBirdSpyDie) || (mouseCount <= 1 && !isMouseSpyDie))
            {
                Debug.Log("한쪽팀이 스파이1명 시민1명일때");
                //둘다 스파이1명, 시민1명이 남았다면 점수로 비교한다.
                if ((birdCount <= 1 && !isBirdSpyDie) && (mouseCount <= 1 && !isMouseSpyDie))
                {
                    Debug.Log("두팀 모두 스파이1명 시민1명일때");
                    if (birdScore > mouseScore)
                    {
                        Debug.Log("두팀 모두 스파이1명 시민1명일때 : 새팀점수 높음");
                        whoWin = (Win.BirdWin);
                    }
                    else if (birdScore < mouseScore)
                    {
                        Debug.Log("두팀 모두 스파이1명 시민1명일때 : 쥐팀점수 높음");
                        whoWin = (Win.MouseWin);
                    }
                    else
                    {
                        whoWin = (Win.Draw);
                    }
                }
                else if ((birdCount <= 1 && !isBirdSpyDie) && !(mouseCount <= 1 && !isMouseSpyDie))
                {
                    Debug.Log("한쪽팀이 스파이1명 시민1명일때 : 쥐팀이 이길만함");
                    whoWin = (Win.MouseWin);
                }
                else if (!(birdCount <= 1 && !isBirdSpyDie) && (mouseCount <= 1 && !isMouseSpyDie))
                {
                    Debug.Log("한쪽팀이 스파이1명 시민1명일때 : 새팀이 이길만함");
                    whoWin = (Win.BirdWin);
                }
                else
                {
                    Debug.Log("뭔가 잘못됨");
                }
            }
            else if (TimeManager.Instance.curRound >= 11) //SettingManager.Instance.maxRoundCount
            {
                Debug.Log("11라운드가 넘었을때");
                if (birdScore > mouseScore)
                {
                    whoWin = (Win.BirdWin);
                }
                else if (birdScore < mouseScore)
                {
                    whoWin = (Win.MouseWin);
                }
                else
                {
                    whoWin = (Win.Draw);
                }
            }

            else//승패를 결정짓는 경우가 아니면 게임 재개
            {
                TimeManager.Instance.FinishScoreTimeSet();
                return;
            }

            photonView.RPC("EndGame", RpcTarget.All, (int)whoWin);
        }

        [PunRPC]
        private void EndGame(int win)
        {
            end = true;

            SoundManager.Instance.bgm.Stop();

            //누가 이겼는지 나타내고 개인 승률에 반영함
            SoundManager.Instance.PlayUISound(UISFXName.Ending);
            //개인 판수 +1

            DataManager.Instance.SaveResult(PlayResult.Play);

            if (PlayGameManager.Instance.myPlayerState.isSpy)
                DataManager.Instance.SaveResult(PlayResult.Spy);

            switch (win)
            {
                case (int)Win.BirdWin:
                    //개인 승수 +1
                    if (PlayGameManager.Instance.myPlayerState.isBird == true && PlayGameManager.Instance.myPlayerState.isSpy == false ||
                        PlayGameManager.Instance.myPlayerState.isBird == false && PlayGameManager.Instance.myPlayerState.isSpy == true)
                        DataManager.Instance.SaveResult(PlayResult.Win);
                    else
                        DataManager.Instance.SaveResult(PlayResult.Lose);
                    //개인 패수 +1
                    birdBackgroundImg.SetActive(true);
                    birdHouseImg.SetActive(true);
                    winText.text = "새팀 승리!";

                    break;
                case (int)Win.MouseWin:
                    //개인 승수 +1
                    if (PlayGameManager.Instance.myPlayerState.isBird == false && PlayGameManager.Instance.myPlayerState.isSpy == false ||
                        PlayGameManager.Instance.myPlayerState.isBird == true && PlayGameManager.Instance.myPlayerState.isSpy == true)
                        DataManager.Instance.SaveResult(PlayResult.Win);
                    else
                        DataManager.Instance.SaveResult(PlayResult.Lose);
                    //개인 패수 +1
                    mouseBackgroundImg.SetActive(true);
                    mouseHouseImg.SetActive(true);
                    winText.text = "쥐팀 승리!";

                    break;
                case (int)Win.Draw:
                    //개인 무승부수 +1
                    DataManager.Instance.SaveResult(PlayResult.Draw);
                    drawBackgroundImg.SetActive(true);
                    winText.text = "무승부!";

                    break;
            }

            //이긴 팀, 스파이 정체 공개 후 방으로 이동
            foreach (PlayerState state in PlayGameManager.Instance.playerList)
            {
                if (state.isBird && state.isSpy)
                    birdSpyImg.sprite = state.sprite;
                else if (!state.isBird && state.isSpy)
                    mouseSpyImg.sprite = state.sprite;
            }

            canvas.SetActive(true);

            //뱃지 얻기
            GetBadge();


            //나가기 버튼 생성
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
                DataManager.Instance.EarnItemToMail("시작의 기쁨");
            else if (DataManager.Instance.myInfo.totalGame == 100)
                DataManager.Instance.EarnItemToMail("낮새밤새");
            else if (DataManager.Instance.myInfo.totalGame == 500)
                DataManager.Instance.EarnItemToMail("게임폐인");

            if (DataManager.Instance.myInfo.win == 10)
                DataManager.Instance.EarnItemToMail("빛나는 승리");
            else if (DataManager.Instance.myInfo.win == 100)
                DataManager.Instance.EarnItemToMail("눈부신 승리");
            else if (DataManager.Instance.myInfo.win == 500)
                DataManager.Instance.EarnItemToMail("황금빛 승리");
        }

        public void OnExitButtonClick()
        {
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.LoadLevel("LobbyTestScene");
            else
                SceneManager.LoadScene("LobbyTestScene");
        }
    }
}
