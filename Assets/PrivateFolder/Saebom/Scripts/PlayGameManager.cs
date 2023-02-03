using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Saebom
{
    [Serializable]
    struct PlayerState
    {
        
        public GameObject playerPrefab;

        public bool isBird;

        public bool isSpy;

        public bool isDie;

    }

    public class PlayGameManager : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> mousePlayerList = new List<GameObject> ();

        [SerializeField]
        private List<GameObject> birdPlayerList = new List<GameObject>();

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

        //게임 시작할 때 초기화

        //게임 시작할 때 플레이어 생성

        //게임 시작할 때 다른 팀원 생성

        //팀원 위치 공유 (방장)

        //현재 점수, 죽은플레이어 값 공유 (방장)

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }

        private void OnEnable()
        {
            //리스트 초기화
            GameStart();
        }

        public void GameStart()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                SetPlayer();
            }

            MakePlayer();

        }

        //방장이 모든 플레이어에게 랜덤으로 역할 부여, playerList 가지고 있기
        private void SetPlayer()
        {
            int teamSum = PhotonNetwork.PlayerList.Length / 2;

            for (int i=0;i<teamSum;i++)
            {
                int random = Random.Range(0, birdPlayerList.Count);
                PlayerState state = new PlayerState();
                state.playerPrefab = birdPlayerList[random];
                state.isBird = true;
                playerList.Add(state);

                birdPlayerList.RemoveAt(random);
            }

            for (int i=teamSum;i<teamSum*2;i++)
            {
                int random = Random.Range(0, mousePlayerList.Count);
                PlayerState state = new PlayerState();
                state.playerPrefab = mousePlayerList[random];
                state.isBird = false;
                playerList.Add(state);

                mousePlayerList.RemoveAt(random);
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
            photonView.RPC("MyPlayerSet", RpcTarget.All, (playerList));

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


        //본인 캐릭터 받아와서 초기화
        [PunRPC]
        private void MyPlayerSet(List<PlayerState> playerList)
        {
            myPlayerState.playerPrefab = playerList[PhotonNetwork.LocalPlayer.ActorNumber].playerPrefab;
            
            if (PhotonNetwork.LocalPlayer.ActorNumber < PhotonNetwork.PlayerList.Length / 2)
            {
                myPlayerState.isBird = true;
            }

        }
    }
}