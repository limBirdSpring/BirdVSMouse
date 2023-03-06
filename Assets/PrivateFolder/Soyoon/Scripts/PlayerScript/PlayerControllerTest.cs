using Cinemachine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Saebom;
using System.Collections;
using System.Collections.Generic;
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

        [Header("Setting")]
        [SerializeField]
        private float moveSpeed = 10;
        [SerializeField]
        private float killCool = 30;
        [SerializeField]
        private float killUpdateTime = 0.05f;

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
        private Collider2D killRangeCollider;
        private WaitForSeconds killUpdateSeconds;
        private Coroutine killCoroutine;

        private int targetInteractionNum;

        private bool isInHouse;

        private Vector2 inputVec;
        public PlayerState state { get; private set; }
        public bool CanKill { get; private set; }
        public float CurKillCoolTime { get; private set; }
        public float KillCoolTime { get { return killCool; } private set { killCool = value; } }
        public int TargetPlayerNum { get; private set; }

        public LinkedList<InterActionAdapter> Interactions { get; private set; }

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
            Interactions = new LinkedList<InterActionAdapter>();
        }

        private void Start()
        {
            if (photonView.IsMine)
            {
                CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
                playerCam.Follow = this.transform;
                playerCam.LookAt = this.transform;
                TargetPlayerNum = 0;
                targetInteractionNum = 0;
                photonView.RPC("SetActiveOrInactive", RpcTarget.All, false);
                killUpdateSeconds = new WaitForSeconds(killUpdateTime);
                CanKill = false;
                if (PlayGameManager.Instance.myPlayerState.isSpy)
                    StartKillCoroutine();
            }

            isInHouse = true;
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
                anim.SetBool("IsDead", true);
                GameObject.Find("KillCanvas").transform.GetChild(0).gameObject.SetActive(true);
                Saebom.PlayGameManager.Instance.gameObject.GetPhotonView().RPC("PlayerDie", RpcTarget.MasterClient, photonView.Owner.GetPlayerNumber());
            }
        }

        [PunRPC]
        public void VoteDie() // 투표로 죽을 시 호출되는 함수
        {
            SetPlayerState(PlayerState.Ghost);
            if (photonView.IsMine)
            {
                anim.SetBool("IsDead", true);
                Saebom.PlayGameManager.Instance.gameObject.GetPhotonView().RPC("PlayerDie", RpcTarget.MasterClient, photonView.Owner.GetPlayerNumber());
            }
        }

        [PunRPC]
        public void CheckIfIsInHouse() // 제한 시간 내 집에 가지 못했을 시 호출되는 함수
        {
            //Debug.LogError("IsInHouse : " + isInHouse + " -> " + this.gameObject.name);
            if (!isInHouse)
            {
                SetPlayerState(PlayerState.Ghost);
                if (photonView.IsMine)
                {
                    anim.SetBool("IsDead", true);
                    Saebom.PlayGameManager.Instance.gameObject.GetPhotonView().RPC("PlayerDie", RpcTarget.MasterClient, photonView.Owner.GetPlayerNumber());
                }
            }
        }

        [PunRPC]
        public void FoundCorpse(int playerNum)
        {
            VoteManager.Instance.FindDeadBody();
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
                OnActive();
            else
                OnInactive();
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
            anim.SetBool("IsActive", false);
            SetPlayerState(PlayerState.Inactive);
        }

        public void OnActive()
        {
            Debug.Log(photonView.Owner.GetPlayerNumber() + "활성화");
            // 활동시기
            Debug.Log(photonView.Owner.GetPlayerNumber() + " 애니메이션 활성화");
            anim.SetBool("IsActive", true);
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
                if (state == PlayerState.Ghost)
                    return;

                if(PlayGameManager.Instance.myPlayerState.isSpy)
                {
                    if ((PlayGameManager.Instance.myPlayerState.isBird && collision.gameObject.name == "BirdHouse")
                        || (!PlayGameManager.Instance.myPlayerState.isBird && collision.gameObject.name == "MouseHouse"))
                    {
                        photonView.RPC("IsInHouse", RpcTarget.All, true);
                        killButtonGray.SetActive(false);
                        killButton.SetActive(false);
                    }

                    if (collision.gameObject.layer == LayerMask.NameToLayer("KillRange"))
                        TargetPlayerNum++;

                    if ((TargetPlayerNum > 0) && killButtonGray.activeSelf)
                        killButton.SetActive(true);
                }
                else
                {
                    if ((PlayGameManager.Instance.myPlayerState.isBird && collision.gameObject.name == "BirdHouse")
                    || (!PlayGameManager.Instance.myPlayerState.isBird && collision.gameObject.name == "MouseHouse"))
                        photonView.RPC("IsInHouse", RpcTarget.All, true);
                }

                if (state == PlayerState.Inactive && (collision.gameObject.name == "MouseCowHouse" || collision.gameObject.name == "BirdCowHouse"
                    || collision.gameObject.name == "Hangari" || collision.gameObject.name == "Cloth" || collision.gameObject.name == "SunMoon" || collision.gameObject.name == "Emergency"))
                {
                    // !Do Nothing
                }
                else if (collision.gameObject.layer != LayerMask.NameToLayer("KillRange"))
                {
                    Debug.Log("enter");
                    InterActionAdapter adapter = null;
                    if (collision.gameObject.name == "CorpseRange")
                        collision.transform.parent.TryGetComponent<InterActionAdapter>(out adapter);
                    else
                        collision.TryGetComponent<InterActionAdapter>(out adapter);

                    if (adapter == null)
                        return;

                    Debug.Log("enter" + collision.gameObject.name);
                    //Debug.LogError("add" + collision.gameObject.name);
                    //Debug.LogError("TargetInteractionNum++");
                    targetInteractionNum++;

                    Interactions.AddLast(adapter);
                    if(!adapter.isActive)
                        Saebom.MissionButton.Instance.MissionButtonOn();
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (photonView.IsMine)
            {
                if (PlayGameManager.Instance.myPlayerState.isSpy)
                {
                    if ((PlayGameManager.Instance.myPlayerState.isBird && collision.gameObject.name == "BirdHouse")
                        || (!PlayGameManager.Instance.myPlayerState.isBird && collision.gameObject.name == "MouseHouse"))
                    {
                        photonView.RPC("IsInHouse",RpcTarget.All, false);
                        killButtonGray.SetActive(true);
                    }

                    if (collision.gameObject.layer == LayerMask.NameToLayer("KillRange"))
                        TargetPlayerNum--;

                    if ((TargetPlayerNum <= 0) && killButtonGray.activeSelf)
                    {
                        TargetPlayerNum = 0;
                        killButton.SetActive(false);
                    }
                }
                else
                {
                    if ((PlayGameManager.Instance.myPlayerState.isBird && collision.gameObject.name == "BirdHouse")
                    || (!PlayGameManager.Instance.myPlayerState.isBird && collision.gameObject.name == "MouseHouse"))
                        photonView.RPC("IsInHouse", RpcTarget.All, false);
                }
                
                if(collision.gameObject.layer != LayerMask.NameToLayer("KillRange"))
                {
                    InterActionAdapter adapter = null;
                    if (collision.gameObject.name == "CorpseRange")
                        collision.transform.parent.TryGetComponent<InterActionAdapter>(out adapter);
                    else
                        collision.TryGetComponent<InterActionAdapter>(out adapter);

                    if (adapter == null)
                        return;

                    Debug.Log("exit" + collision.gameObject.name);
                    //Debug.LogError("delete" + collision.gameObject.name);
                    //Debug.LogError("TargetInteractionNum--");
                    targetInteractionNum--;

                    Interactions.Remove(adapter);
                    if(targetInteractionNum <= 0)
                    {
                        targetInteractionNum = 0;
                        Interactions.Clear();
                        Saebom.MissionButton.Instance.MissionButtonOff();
                    }
                }
            }
        }

        [PunRPC]
        public void IsInHouse(bool inHouse)
        {
            isInHouse = inHouse;
        }

        public void StartKillCoroutine()
        {
            //Debug.LogError("킬 코루틴 시작");
            StopAllCoroutines();
            CurKillCoolTime = 0;
            killCoroutine = StartCoroutine(KillCoolTimeUpdate());
        }

        public void StopKillCoroutine()
        {
            //Debug.LogError("킬 코루틴 종료");
            CurKillCoolTime = 0;
            StopCoroutine(killCoroutine);
        }

        public IEnumerator KillCoolTimeUpdate()
        {
            //Debug.LogError("킬 쿨타임 시작");
            CanKill = false;
            float killTime = killCool * (1 / killUpdateTime);
            for(int i=0;i<killTime;i++)
            {
                yield return killUpdateSeconds;
                CurKillCoolTime += killUpdateTime;
            }
            CanKill = true;
            CurKillCoolTime = 0;
            //Debug.LogError("킬 쿨타임 종료");
        }
    }
}
