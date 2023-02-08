using Cinemachine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

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

        private Joystick joystick;
        private CullingMaskController cullingMask;
        private GameObject killButtonGray;
        private GameObject killButton;
        private GameObject targetPlayer;
        private Collider2D killRangeCollider;

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
            cullingMask = Camera.main.GetComponent<CullingMaskController>();
            killButtonGray = uiCanvas.GetChild(10).gameObject;
            killButton = uiCanvas.GetChild(11).gameObject;
            killRangeCollider = gameObject.transform.GetChild(1).GetComponent<Collider2D>();
            //Debug.Log(killRangeCollider.name);
        }

        private void Start()
        {
            if(photonView.IsMine)
            {                
                // ScoreManager.Instance.player = this;
                CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
                playerCam.Follow = this.transform;
                playerCam.LookAt = this.transform;
                targetPlayer = null;
                //if(PlayGameManager.Instance.myPlayerState.isSpy)
                //    killButtonGray.SetActive(true);
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

        [PunRPC]
        public void Die()
        {
            Instantiate(death, transform.position, Quaternion.identity);
            anim.SetTrigger("isDeath");
            SetPlayerState(PlayerState.Ghost);
            Saebom.PlayGameManager.Instance.PlayerDie(photonView.Owner.GetPlayerNumber());
        }

        private void SetNamePosition()
        {
            nameTransform.anchoredPosition = namePosition;
        }

        public void OnInactive()
        {
            // 비활동시기(내 활동시간이 아닐경우)
            anim.SetTrigger("IsInactive");
            SetPlayerState(PlayerState.Inactive);

        }

        public void OnActive()
        {
            // 활동시기
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
                    if (photonView.IsMine)
                    {
                        cullingMask.OffLayerMask(LayerMask.NameToLayer("InActive"));
                        cullingMask.OffLayerMask(LayerMask.NameToLayer("Ghost"));
                        cullingMask.OffLayerMask(LayerMask.NameToLayer("Shadow"));
                    }
                    SetKillRange();
                    break;
                case PlayerState.Inactive:
                    if (photonView.IsMine)
                    {
                        SetLayer(LayerMask.NameToLayer("InActive"));
                        cullingMask.OnLayerMask(LayerMask.NameToLayer("InActive"));
                    }
                    SetKillRange();
                    break;
                case PlayerState.Ghost:
                    SetLayer(LayerMask.NameToLayer("Ghost"));
                    SetNamePosition();
                    if (photonView.IsMine)
                    {
                        cullingMask.OnLayerMask(LayerMask.NameToLayer("Ghost"));
                        cullingMask.OnLayerMask(LayerMask.NameToLayer("Shadow"));
                        cullingMask.OnLayerMask(LayerMask.NameToLayer("InActive"));
                    }
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
                if (child.gameObject.name == "KillRangeCollider")
                    child.gameObject.layer = LayerMask.NameToLayer("KillRange");
            }
        }

        private void SetKillRange()
        {
            Debug.Log("SetKillRange");
            if (state == PlayerState.Active)
                killRangeCollider.gameObject.SetActive(true);
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
                    killButton.GetComponent<KillButton>().target = targetPlayer.transform.parent.gameObject;
                    return;
                }
                else if (collision.gameObject.layer != LayerMask.NameToLayer("KillRange"))
                {
                    Debug.Log("enter" + collision.gameObject.name);
                    Saebom.MissionButton.Instance.inter = collision.GetComponent<InterActionAdapter>();
                    Saebom.MissionButton.Instance.MissionButtonOn();
                }
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
                else if (collision.gameObject.layer != LayerMask.NameToLayer("KillRange"))
                {
                    Debug.Log("exit" + collision.gameObject.name);
                    Saebom.MissionButton.Instance.MissionButtonOff();
                }
            }
        }
    }
}
