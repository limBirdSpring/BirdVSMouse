using Cinemachine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Saebom;
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

        private bool isInHouse;

        private Vector2 inputVec;
        public PlayerState state { get; private set; }

        private void Awake()
        {
            // 변수 세팅
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
        }

        private void Start()
        {
            if (photonView.IsMine)
            {
                // ScoreManager.Instance.player = this;
                CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
                playerCam.Follow = this.transform;
                playerCam.LookAt = this.transform;
                targetPlayer = null;
                photonView.RPC("SetActiveOrInactive", RpcTarget.All, false);
            }

            isInHouse = false;
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                Move();
                photonView.RPC("Flip", RpcTarget.All, rigid.velocity.x);
                AnimatorUpdate();
            }
        }

        private void Move()
        {
            inputVec = joystick.inputVec;

            rigid.velocity = inputVec * moveSpeed;
        }

        [PunRPC]
        public void Flip(float velocity)
        {
            if (velocity > 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (velocity < 0)
            {
                spriteRenderer.flipX = false;
            }
        }

        private void AnimatorUpdate()
        {
            anim.SetFloat("Speed", rigid.velocity.sqrMagnitude);
        }

        [PunRPC]
        public void Die() // 스파이에게 죽을 시 호출되는 함수
        {
            GameObject corpse = Instantiate(death, transform.position, Quaternion.identity);
            corpse.name = "Corpse";
            corpse.tag = corpse.name;
            Corpse targetCorpse = corpse.GetComponent<Corpse>();
            targetCorpse.playerNum = photonView.Owner.GetPlayerNumber();
            SetPlayerState(PlayerState.Ghost);
            // TODO : 킬되는 화면 뜨는 것 구현
            if (photonView.IsMine)
            {
                anim.SetTrigger("IsDeath");
                GameObject.Find("KillCanvas").transform.GetChild(0).gameObject.SetActive(true);
                Saebom.PlayGameManager.Instance.PlayerDie(photonView.Owner.GetPlayerNumber());
            }
        }

        [PunRPC]
        public void VoteDie() // 투표로 죽을 시 호출되는 함수
        {
            SetPlayerState(PlayerState.Ghost);
            if (photonView.IsMine)
            {
                anim.SetTrigger("IsDeath");
                Saebom.PlayGameManager.Instance.PlayerDie(photonView.Owner.GetPlayerNumber());
            }
        }

        [PunRPC]
        public void CheckIfIsInHouse() // 제한 시간 내 집에 가지 못했을 시 호출되는 함수
        {
            if (!isInHouse)// && state == PlayerState.Active)
            {
                GameObject corpse = Instantiate(death, transform.position, Quaternion.identity);
                corpse.name = "Corpse";
                corpse.tag = corpse.name;
                Corpse targetCorpse = corpse.GetComponent<Corpse>();
                targetCorpse.playerNum = photonView.Owner.GetPlayerNumber();
                SetPlayerState(PlayerState.Ghost);
                if (photonView.IsMine)
                {
                    anim.SetTrigger("IsDeath");
                    Saebom.PlayGameManager.Instance.PlayerDie(photonView.Owner.GetPlayerNumber());
                }
            }
        }

        [PunRPC]
        public void FoundCorpse(int playerNum)
        {
            GameObject.Find("VoteCanvas").transform.GetChild(0).gameObject.SetActive(true);
        }

        [PunRPC]
        public void DestroyCorpse(int playerNum, bool all = false)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Corpse");

            foreach(GameObject obj in objs)
            {
                if (!all)
                {
                    if (obj.GetComponent<Corpse>().playerNum == playerNum)
                    {
                        Destroy(obj);
                        return;
                    }
                }
                else Destroy(obj);
            }
        }

        [PunRPC]
        public void SetActiveOrInactive(bool turnToNight)
        {
            if (state == PlayerState.Ghost)
                return;

            Debug.Log("player num : " + photonView.Owner.GetPlayerNumber());
            Debug.Log("is bird : " + PlayGameManager.Instance.playerList[photonView.Owner.GetPlayerNumber()].isBird);
            Debug.Log("turn to night : " + turnToNight);
            if (PlayGameManager.Instance.playerList[photonView.Owner.GetPlayerNumber()].isBird && !turnToNight)
                OnActive();
            else if (!(PlayGameManager.Instance.playerList[photonView.Owner.GetPlayerNumber()].isBird) && turnToNight)
            {
                anim.SetTrigger("IsActive");
                OnActive();
            }
            else
            {
                anim.SetTrigger("IsInactive");
                OnInactive();
            }
        }

        private void SetNamePosition()
        {
            nameTransform.anchoredPosition = namePosition;
        }

        public void OnInactive()
        {
            Debug.Log(photonView.Owner.GetPlayerNumber() + "비활성화");
            // 비활동시기(내 활동시간이 아닐경우)
            Debug.Log(photonView.Owner.GetPlayerNumber() + " 애니메이션 비활성화");
            SetPlayerState(PlayerState.Inactive);
        }

        public void OnActive()
        {
            Debug.Log(photonView.Owner.GetPlayerNumber() + "활성화");
            // 활동시기
            Debug.Log(photonView.Owner.GetPlayerNumber() + " 애니메이션 활성화");
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
                    SetLayer(LayerMask.NameToLayer("InActive"));
                    if (photonView.IsMine)
                    {
                        cullingMask.OnLayerMask(LayerMask.NameToLayer("InActive"));
                        cullingMask.OffLayerMask(LayerMask.NameToLayer("Ghost"));
                        cullingMask.OffLayerMask(LayerMask.NameToLayer("Shadow"));
                    }
                    SetKillRange();
                    break;
                case PlayerState.Ghost:
                    SetLayer(LayerMask.NameToLayer("Ghost"));
                    SetNamePosition();
                    if (photonView.IsMine)
                    {
                        cullingMask.OnLayerMask(LayerMask.NameToLayer("InActive"));
                        cullingMask.OnLayerMask(LayerMask.NameToLayer("Ghost"));
                        cullingMask.OnLayerMask(LayerMask.NameToLayer("Shadow"));
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
                if ((PlayGameManager.Instance.myPlayerState.isBird && collision.gameObject.name == "BirdHouse")
                    || (!PlayGameManager.Instance.myPlayerState.isBird && collision.gameObject.name == "MouseHouse"))
                    isInHouse = true;

                if (state == PlayerState.Ghost)
                    return;

                if (collision.gameObject.layer == LayerMask.NameToLayer("KillRange") && killButtonGray.activeSelf)
                {
                    killButton.SetActive(true);
                    targetPlayer = collision.gameObject;
                    killButton.GetComponent<KillButton>().target = targetPlayer.transform.parent.gameObject;
                    return;
                }
                else if(state == PlayerState.Inactive && (collision.gameObject.name == "MouseCowHouse" || collision.gameObject.name == "BirdCowHouse"
                    || collision.gameObject.name == "Hangari" || collision.gameObject.name == "Cloth" || collision.gameObject.name == "SunMoon" || collision.gameObject.name == "Bag" || collision.gameObject.name == "Emergency"))
                {
                    // !Do Nothing
                }
                else if(collision.gameObject.layer != LayerMask.NameToLayer("CorpseRange"))
                {
                    Debug.Log("enter" + collision.gameObject.name);
                    Saebom.MissionButton.Instance.inter = collision.GetComponent<InterActionAdapter>();
                    Saebom.MissionButton.Instance.MissionButtonOn();
                }
            }
        }
        
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (photonView.IsMine)
            {
                if ((PlayGameManager.Instance.myPlayerState.isBird && collision.gameObject.name == "BirdHouse")
                    || (!PlayGameManager.Instance.myPlayerState.isBird && collision.gameObject.name == "MouseHouse"))
                    isInHouse = false;

                if (collision.gameObject.layer == LayerMask.NameToLayer("KillRange") && killButtonGray.activeSelf)
                {
                    killButton.SetActive(false);
                    targetPlayer = null;
                    return;
                }
                else if(collision.gameObject.layer != LayerMask.NameToLayer("CorpseRange"))
                {
                    Debug.Log("exit" + collision.gameObject.name);
                    Saebom.MissionButton.Instance.MissionButtonOff();
                }
            }
        }
    }
}
