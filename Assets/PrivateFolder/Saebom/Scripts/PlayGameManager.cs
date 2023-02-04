using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

namespace Saebom
{
    [Serializable]
    struct PlayerState
    {
        public int num;

        public string name;
        
        public GameObject playerPrefab;

        public Sprite sprite;

        public bool isBird;

        public bool isSpy;

        public bool isDie;

    }

    public class PlayGameManager : MonoBehaviour
    {
        [SerializeField]
        private List<PlayerState> mouseJobList = new List<PlayerState> ();

        [SerializeField]
        private List<PlayerState> birdJobList = new List<PlayerState>();

        private List<PlayerState> playerList = new List<PlayerState>();

        //===개인의 정보===
        private PlayerState myPlayerState;
      
        //================

        [SerializeField]
        private Transform mouseHouse;

        [SerializeField]
        private Transform birdHouse;

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
        }

       // private void OnEnable()
       // {
       //     //리스트 초기화
       //     GameStart();
       // }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                //리스트 초기화
                GameStart();
            }
        }

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

            for (int i=0;i<teamSum;i++)
            {
                int random = Random.Range(0, mouseJobList.Count);
                playerList[i] = mouseJobList[random];
                mouseJobList.RemoveAt(random);
            }

            for (int i=teamSum;i<teamSum*2;i++)
            {
                int random = Random.Range(0, birdJobList.Count);
                playerList[i] = mouseJobList[random];
                mouseJobList.RemoveAt(random);
            }

            int birdSpy = Random.Range(0, teamSum);
            int mouseSpy = Random.Range(teamSum, teamSum * 2);

            PlayerState bird = playerList[birdSpy];
            bird.isSpy = true;
            playerList[birdSpy] = bird;

            PlayerState mouse = playerList[mouseSpy];
            mouse.isSpy = true;
            playerList[mouseSpy] = bird;

            //각자의 펀 함수 소환
            for (int i = 0; i < playerList.Count; i++)
            {
                photonView.RPC("MyPlayerSet", RpcTarget.All, (i, playerList[i].num, playerList[i].isBird, playerList[i].isSpy));
            }
        }

        private void SetReadyScene()
        {
            readyScene.SetActive(true);

            if (!myPlayerState.isSpy)
            {
                readyJobText.text = "당신은 " + myPlayerState.name + " 입니다.";
                readyPlayerImage.sprite = myPlayerState.sprite;
            }
            else
            {
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

            StartCoroutine(SetReadySceneCor());
        }

        private IEnumerator SetReadySceneCor()
        {
            yield return new WaitForSeconds(3f);
            MakePlayer();
            readyScene.SetActive(false);
        }

        //본인 캐릭터 받아와서 초기화
        [PunRPC]
        private void MyPlayerSet(int i, int jobNum, bool isBird, bool isSpy)
        {
            if (photonView.Owner.ActorNumber != i)
                return;

            Debug.Log("개인 플레이어 세팅");
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
            if(myPlayerState.isBird)
                PhotonNetwork.Instantiate(myPlayerState.playerPrefab.name, birdHouse.position, Quaternion.identity);
            else
                PhotonNetwork.Instantiate(myPlayerState.playerPrefab.name, mouseHouse.position, Quaternion.identity);

            if (myPlayerState.isSpy)
                killButtonGray.SetActive(true);
        }


    }
}