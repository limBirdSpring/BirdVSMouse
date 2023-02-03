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

        //===������ ����===
        private PlayerState myPlayerState;
      
        //================

        [SerializeField]
        private Transform mouseHouse;

        [SerializeField]
        private Transform birdHouse;

        [SerializeField]
        private GameObject killButtonGray;


        private PhotonView photonView;

        //���� ������ �� �ʱ�ȭ

        //���� ������ �� �÷��̾� ����

        //���� ������ �� �ٸ� ���� ����

        //���� ��ġ ���� (����)

        //���� ����, �����÷��̾� �� ���� (����)

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }

        private void OnEnable()
        {
            //����Ʈ �ʱ�ȭ
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

        //������ ��� �÷��̾�� �������� ���� �ο�, playerList ������ �ֱ�
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

            //������ �� �Լ� ��ȯ
            photonView.RPC("MyPlayerSet", RpcTarget.All, (playerList));

        }

        //�÷��̾� ���� : �������� ��� ų��ư Ȱ��ȭ
        private void MakePlayer()
        {
            if(myPlayerState.isBird)
                PhotonNetwork.Instantiate(myPlayerState.playerPrefab.name, birdHouse.position, Quaternion.identity);
            else
                PhotonNetwork.Instantiate(myPlayerState.playerPrefab.name, mouseHouse.position, Quaternion.identity);

            if (myPlayerState.isSpy)
                killButtonGray.SetActive(true);
        }


        //���� ĳ���� �޾ƿͼ� �ʱ�ȭ
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