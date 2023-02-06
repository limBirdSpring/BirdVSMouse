using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;
using Photon.Pun;
using Cinemachine;
using Photon.Realtime;

//public enum PlayerState { Active, Inactive, Ghost }

namespace SoYoon
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerControllerTest : MonoBehaviourPun
    {
        private Rigidbody2D rigid;
        private SpriteRenderer spriteRenderer;
        private Animator anim;

        [SerializeField]
        private float moveSpeed = 10;

        [Header("Ghost")]
        [SerializeField]
        private GameObject death;
        [SerializeField]
        private Vector3 namePosition = new Vector3(0, 2, 0);
        private RectTransform nameTransform;

        [Header("IsSpy")]
        [SerializeField]
        private Collider2D killRangeCollider;

        private Joystick joystick;
        private GameObject killButtonGray; // ¿”Ω√
        private GameObject killButton;
        private GameObject targetPlayer;

        private Vector2 inputVec;
        public PlayerState state { get; private set; } = PlayerState.Active;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            anim = GetComponentInChildren<Animator>();
            rigid = GetComponent<Rigidbody2D>();
            nameTransform = GetComponentInChildren<RectTransform>();
            Transform uiCanvas = GameObject.Find("UICanvas").transform;
            joystick = uiCanvas.GetChild(9).GetComponent<Joystick>();
            killButtonGray = uiCanvas.GetChild(10).gameObject;
            killButton = uiCanvas.GetChild(11).gameObject;
        }

        private void Start()
        {
            if(photonView.IsMine)
            {
                CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
                playerCam.Follow = this.transform;
                playerCam.LookAt = this.transform;
                targetPlayer = null;
                //if(PlayGameManager.Instance.myPlayerState.isSpy)
                    //killButtonGray.SetActive(true);
            }
            SetPlayerState(PlayerState.Active);
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                Move();
                AnimatorUpdate();
            }
        }

        private void Move()
        {
            inputVec = joystick.inputVec;

            rigid.velocity = inputVec * moveSpeed;

            if (rigid.velocity.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (rigid.velocity.x < 0)
            {
                spriteRenderer.flipX = false;
            }
        }

        private void AnimatorUpdate()
        {
            anim.SetFloat("Speed", rigid.velocity.sqrMagnitude);
        }

        public void Die()
        {
            Instantiate(death, transform.position, death.transform.rotation);
            anim.SetTrigger("isDeath");
            SetPlayerState(PlayerState.Ghost);
            SetNamePosition();
        }

        private void SetNamePosition()
        {
            nameTransform.anchoredPosition = namePosition;
        }

        public void OnInactive()
        {
            //TODO :  ?????? ?©£??ª◊ ???? ???
            anim.SetTrigger("IsInactive");
            SetPlayerState(PlayerState.Inactive);

        }

        public void OnActive()
        {
            //TODO :  ???? ?©£??ª◊ ???? ???
            anim.SetTrigger("IsActive");
            SetPlayerState(PlayerState.Active);
        }

        private void SetPlayerState(PlayerState playerState)
        {
            state = playerState;
            switch (state)
            {
                case PlayerState.Active:
                    SetLayer(LayerMask.NameToLayer("Player"));
                    SetKillRange();
                    break;
                case PlayerState.Inactive:
                    SetLayer(LayerMask.NameToLayer("Ghost"));
                    SetKillRange();
                    break;
                case PlayerState.Ghost:
                    SetLayer(LayerMask.NameToLayer("Ghost"));
                    SetKillRange();
                    break;
            }

        }
        private void SetLayer(LayerMask layer)
        {
            gameObject.layer = layer;
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = layer;
            }
        }

        private void SetKillRange()
        {
            if (state == PlayerState.Active)
            {
                killRangeCollider.gameObject.SetActive(true);
                killRangeCollider.gameObject.layer = LayerMask.NameToLayer("KillRange");
            }
            else
                killRangeCollider.gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (photonView.IsMine)
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("KillRange") && killButtonGray.activeSelf)
                {
                    killButton.SetActive(true);
                    targetPlayer = collision.gameObject;
                    killButton.GetComponent<KillButton>().target = targetPlayer;
                    return;
                }
                Debug.Log("enter" + collision.gameObject.name);
                Saebom.MissionButton.Instance.inter = collision.GetComponent<InterActionAdapter>();
                Saebom.MissionButton.Instance.MissionButtonOn();
            }

            //if (collision.gameObject.layer == LayerMask.NameToLayer("KillRange") && killButtonGray.activeSelf)
            //{
            //    foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            //    {
            //        if (player.ActorNumber == collision.gameObject.GetComponent<PlayerControllerTest>().photonView.Owner.ActorNumber)
            //        {
            //            targetPlayer = player;
            //            break;
            //        }
            //    }
            //}
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (photonView.IsMine)
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("KillRange") && killButtonGray.activeSelf)
                {
                    killButton.SetActive(false);
                    return;
                }
                Debug.Log("exit" + collision.gameObject.name);
                Saebom.MissionButton.Instance.MissionButtonOff();
            }
        }
    }
}
