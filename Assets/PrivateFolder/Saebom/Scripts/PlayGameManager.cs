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

       // private void OnEnable()
       // {
       //     //����Ʈ �ʱ�ȭ
       //     GameStart();
       // }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                //����Ʈ �ʱ�ȭ
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

        //������ ��� �÷��̾�� �������� ���� �ο�, playerList ������ �ֱ�
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

            //������ �� �Լ� ��ȯ
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
                readyJobText.text = "����� " + myPlayerState.name + " �Դϴ�.";
                readyPlayerImage.sprite = myPlayerState.sprite;
            }
            else
            {
                string team;
                if (myPlayerState.isBird)
                    team = "����";
                else
                    team = "����";
                    
                readyJobText.text = "����� " + myPlayerState.name + "�� ������ " + team + "�������Դϴ�.";
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

        //���� ĳ���� �޾ƿͼ� �ʱ�ȭ
        [PunRPC]
        private void MyPlayerSet(int i, int jobNum, bool isBird, bool isSpy)
        {
            if (photonView.Owner.ActorNumber != i)
                return;

            Debug.Log("���� �÷��̾� ����");
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