using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using SoYoon;

namespace Saebom
{
    [Serializable]
    public struct PlayerState
    {
        public int jobNum;

        public string name;

        public GameObject playerPrefab;

        public Sprite sprite;

        public bool isBird;

        public bool isSpy;

        public bool isDie;

    }

    public class PlayGameManager : SingleTon<PlayGameManager>
    {
        [SerializeField]
        private List<PlayerState> mouseJobList = new List<PlayerState>();

        [SerializeField]
        private List<PlayerState> birdJobList = new List<PlayerState>();

        //방장이 가지고있는 플레이어리스트로 몇번플레이어가 무슨역할인지, 죽었는지 모두 알수있다.
        public List<PlayerState> playerList= new List<PlayerState>();

        public List<PlayerControllerTest> playerController = new List<PlayerControllerTest>();

        //===개인의 정보===
        public PlayerState myPlayerState;
        //해당 struct를 통해 나의 직업 및 상태 판단 가능

        //================

        public Transform mouseHouse;

        public Transform birdHouse;

        [SerializeField]
        private GameObject killButtonGray;


        private PhotonView photonView;


        //===============

        [SerializeField]
        private GameObject readyScene;

        [SerializeField]
        private TextMeshProUGUI readyJobText;

        [SerializeField]
        private Image readyPlayerImage;

        //===============

        //방장만 업데이트
        private TimeManager time;


        //게임 시작할 때 초기화

        //게임 시작할 때 플레이어 생성

        //게임 시작할 때 다른 팀원 생성

        //팀원 위치 공유 (방장)

        //현재 점수, 죽은플레이어 값 공유 (방장)

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
            Destroy(GameObject.Find("LobbyManager"));
        }

      private void OnEnable()
      {
          //리스트 초기화
          GameStart();
      }

       //private void Update()
       //{
       //    if (Input.GetKeyDown(KeyCode.F1))
       //    {
       //        //리스트 초기화
       //        GameStart();
       //    }
       //}

        public void GameStart()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                SetPlayer();
            }

            
            StartCoroutine(GameStartCor());
        }

        private IEnumerator GameStartCor()
        {
            yield return new WaitForSeconds(2f);
            SetReadyScene();
        }

        //방장이 모든 플레이어에게 랜덤으로 역할 부여, playerList 가지고 있기
        private void SetPlayer()
        {
            int teamSum = PhotonNetwork.PlayerList.Length / 2;

            for (int i = 0; i < teamSum; i++)
            {
                playerList.Add(mouseJobList[i]);
            }

            for (int i = teamSum; i < teamSum * 2; i++)
            {
                playerList.Add(birdJobList[i-teamSum]);
            }

            int birdSpy = Random.Range(0, teamSum);
            int mouseSpy = Random.Range(teamSum, teamSum * 2);

            PlayerState bird = playerList[birdSpy];
            bird.isSpy = true;
            playerList[birdSpy] = bird;

            PlayerState mouse = playerList[mouseSpy];
            mouse.isSpy = true;
            playerList[mouseSpy] = mouse;


            for (int i = 0; i < teamSum*2; i++)
            {
                //셔플
                int random = Random.Range(0, playerList.Count);
                PlayerState player = playerList[random];
                playerList[random] = playerList[i];
                playerList[i] = player;
            }


           // //각자의 펀 함수 소환
            for (int i = 0; i < playerList.Count; i++)
            {
                photonView.RPC("MyPlayerSet", RpcTarget.All, i, playerList[i].jobNum, playerList[i].isBird, playerList[i].isSpy);
                
            }
        }

        private void SetReadyScene()
        {
            readyScene.SetActive(true);

            SoundManager.Instance.PlayUISound(UISFXName.GetJob);

            if (!myPlayerState.isSpy)
            {
                readyJobText.text = "당신은 " + myPlayerState.name + " 입니다.";
                readyPlayerImage.sprite = myPlayerState.sprite;
            }
            else
            {
                readyJobText.color = new Color(255, 0, 0, 1);

                string team;
                if (myPlayerState.isBird)
                    team = "쥐팀";
                else
                    team = "새팀";

                readyJobText.text = "당신은 " + myPlayerState.name + "로 위장한 " + team + "스파이입니다.";
                readyPlayerImage.sprite = myPlayerState.sprite;
            }

            readyJobText.gameObject.SetActive(true);
            readyPlayerImage.gameObject.SetActive(true);

            //미션 나눠주기
            MissionButton.Instance.MissionShare();

            StartCoroutine(SetReadySceneCor());
        }

        private IEnumerator SetReadySceneCor()
        {
            yield return new WaitForSeconds(3f);
            MakePlayer();
            readyScene.SetActive(false);
            TimeManager.Instance.TimeOn();
        }

        //본인 캐릭터 받아와서 초기화
        [PunRPC]
        public void MyPlayerSet(int i, int jobNum, bool isBird, bool isSpy)
        {
            Debug.Log("개인 플레이어 세팅");

            if (!PhotonNetwork.IsMasterClient)
            {
                if (isBird)
                {
                    PlayerState playerState = birdJobList[jobNum];
                    playerState.isSpy = isSpy;
                    playerList.Add(playerState);
                }
                else
                {
                    PlayerState playerState = mouseJobList[jobNum];
                    playerState.isSpy = isSpy;
                    playerList.Add(playerState);
                }
            }

            Debug.LogError(PhotonNetwork.LocalPlayer.GetPlayerNumber());

            if (PhotonNetwork.LocalPlayer.GetPlayerNumber() != i)
                return;


            if (isBird)
            {
                myPlayerState = birdJobList[jobNum];
                myPlayerState.isSpy = isSpy;
            }
            else
            {
                myPlayerState = mouseJobList[jobNum];
                myPlayerState.isSpy = isSpy;
            }

        }

        //플레이어 생성 : 스파이일 경우 킬버튼 활성화
        private void MakePlayer()
        {
            GameObject player;

            if (myPlayerState.isBird)
                player = PhotonNetwork.Instantiate(myPlayerState.playerPrefab.name, birdHouse.position, Quaternion.identity);
            else
                player = PhotonNetwork.Instantiate(myPlayerState.playerPrefab.name, mouseHouse.position, Quaternion.identity);

            if (myPlayerState.isSpy)
                killButtonGray.SetActive(true);

            myPlayerState.playerPrefab = player;

            photonView.RPC("MakePlayerSaveToPlayerList", RpcTarget.All, player.name, PhotonNetwork.LocalPlayer.GetPlayerNumber());
            
        }

        //생성된 플레이어를 플레이어리스트에 저장
        [PunRPC]
        public void MakePlayerSaveToPlayerList(string playerName, int index)
        {
            PlayerState state = playerList[index];
            state.playerPrefab = GameObject.Find(playerName);
            playerList[index] = state;
        }



        //플레이어가 투표로 죽었을 때 호출할 함수
        public void PlayerDie(int index)
        {
            photonView.RPC("PlayerDieAndMasterPlayerListUpdate", RpcTarget.MasterClient, index);

            if (PhotonNetwork.IsMasterClient)
                ScoreManager.Instance.MasterCurPlayerStateUpdate();
        }

        [PunRPC]
        public void PlayerDieAndMasterPlayerListUpdate(int index)
        {
            PlayerState player = playerList[index];
            player.isDie = true;
            playerList[index]= player;

            for (int i = 0; i < playerList.Count; i++)
            {
                //갱신된 플레이어 전달
                photonView.RPC("PlayerListSet", RpcTarget.All, i, playerList[i].jobNum, playerList[i].isBird, playerList[i].isDie);
            }
        }

        [PunRPC]
        public void PlayerListSet(int i, int jobNum, bool isBird, bool isDie)
        {
            Debug.Log("개인 플레이어 세팅");

            if (isDie)
            {
                PlayerState state = playerList[i];
                state.isDie = true;
                playerList[i] = state;
            }

            if (PhotonNetwork.LocalPlayer.GetPlayerNumber() != i)
                return;


            myPlayerState = playerList[i];

        }


        public void PlayerGoHomeNow()
        {
            if (myPlayerState.isBird)
            {
                myPlayerState.playerPrefab.gameObject.transform.position = birdHouse.position;
            }
            else
            {
                myPlayerState.playerPrefab.gameObject.transform.position = mouseHouse.position;
            }
        }




    }
}
