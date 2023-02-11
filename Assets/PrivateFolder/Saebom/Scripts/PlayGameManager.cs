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

        //������ �������ִ� �÷��̾��Ʈ�� ����÷��̾ ������������, �׾����� ��� �˼��ִ�.
        public List<PlayerState> playerList= new List<PlayerState>();

        public List<PlayerControllerTest> playerController = new List<PlayerControllerTest>();

        //===������ ����===
        public PlayerState myPlayerState;
        //�ش� struct�� ���� ���� ���� �� ���� �Ǵ� ����

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
            Destroy(GameObject.Find("LobbyManager"));
        }

      private void OnEnable()
      {
          //����Ʈ �ʱ�ȭ
          GameStart();
      }

       //private void Update()
       //{
       //    if (Input.GetKeyDown(KeyCode.F1))
       //    {
       //        //����Ʈ �ʱ�ȭ
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

        //������ ��� �÷��̾�� �������� ���� �ο�, playerList ������ �ֱ�
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
                //����
                int random = Random.Range(0, playerList.Count);
                PlayerState player = playerList[random];
                playerList[random] = playerList[i];
                playerList[i] = player;
            }


           // //������ �� �Լ� ��ȯ
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
                readyJobText.text = "����� " + myPlayerState.name + " �Դϴ�.";
                readyPlayerImage.sprite = myPlayerState.sprite;
            }
            else
            {
                readyJobText.color = new Color(255, 0, 0, 1);

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

            //�̼� �����ֱ�
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

        //���� ĳ���� �޾ƿͼ� �ʱ�ȭ
        [PunRPC]
        public void MyPlayerSet(int i, int jobNum, bool isBird, bool isSpy)
        {
            Debug.Log("���� �÷��̾� ����");

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

        //�÷��̾� ���� : �������� ��� ų��ư Ȱ��ȭ
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

        //������ �÷��̾ �÷��̾��Ʈ�� ����
        [PunRPC]
        public void MakePlayerSaveToPlayerList(string playerName, int index)
        {
            PlayerState state = playerList[index];
            state.playerPrefab = GameObject.Find(playerName);
            playerList[index] = state;
        }



        //�÷��̾ ��ǥ�� �׾��� �� ȣ���� �Լ�
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
                //���ŵ� �÷��̾� ����
                photonView.RPC("PlayerListSet", RpcTarget.All, i, playerList[i].jobNum, playerList[i].isBird, playerList[i].isDie);
            }
        }

        [PunRPC]
        public void PlayerListSet(int i, int jobNum, bool isBird, bool isDie)
        {
            Debug.Log("���� �÷��̾� ����");

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
