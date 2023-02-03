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
        public string name;
        
        public GameObject playerPrefab;

        public bool isBird;

        public bool isSpy;

        public bool isDie;

    }

    public class PlayGameManager : MonoBehaviour
    {
        [SerializeField]
        private List<PlayerState> mousePlayerList = new List<PlayerState> ();

        [SerializeField]
        private List<PlayerState> birdPlayerList = new List<PlayerState>();

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


        //===============

        [SerializeField]
        private GameObject readyScene;

        [SerializeField]
        private TextMeshProUGUI readyJobText;

        [SerializeField]
        private Image readyPlayerImage;

        //===============

        //���常 ������Ʈ
        private TimeManager time;


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

            SetReadyScene();

        }

        //������ ��� �÷��̾�� �������� ���� �ο�, playerList ������ �ֱ�
        private void SetPlayer()
        {
            int teamSum = PhotonNetwork.PlayerList.Length / 2;

            for (int i=0;i<teamSum;i++)
            {
                int random = Random.Range(0, birdPlayerList.Count);
                playerList.Add(birdPlayerList[random]);

                birdPlayerList.RemoveAt(random);
            }

            for (int i=teamSum;i<teamSum*2;i++)
            {
                int random = Random.Range(0, mousePlayerList.Count);
                PlayerState state = new PlayerState();
                playerList.Add(mousePlayerList[random]);

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

        private void SetReadyScene()
        {
            readyScene.SetActive(true);

            if (!myPlayerState.isSpy)
            {
                readyJobText.text = "����� " + myPlayerState.name + " �Դϴ�.";
                readyPlayerImage.sprite = myPlayerState.playerPrefab.GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                string team;
                if (myPlayerState.isBird)
                    team = "����";
                else
                    team = "����";
                    
                readyJobText.text = "����� " + myPlayerState.name + "�� ������ " + team + "�������Դϴ�.";
                readyPlayerImage.sprite = myPlayerState.playerPrefab.GetComponent<SpriteRenderer>().sprite;
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


    }
}