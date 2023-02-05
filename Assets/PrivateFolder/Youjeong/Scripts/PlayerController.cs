using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;
using Photon.Pun;

public enum PlayerState { Active, Inactive, Ghost}

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private PhotonView photonView;

    [SerializeField]
    private Joystick joystick;
    [SerializeField]
    private float moveSpeed=10;
    [SerializeField]
    private CullingMaskController cullingMask;

    [Header("Ghost")]
    [SerializeField]
    private Collider2D colli;
    [SerializeField]
    private Vector3 namePosition = new Vector3(0, 2, 0);
    private RectTransform nameTransform;

    private Vector2 inputVec;
    public PlayerState state { get; private set; } = PlayerState.Active;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        spriteRenderer=GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        nameTransform = GetComponentInChildren<RectTransform>();
        SetPlayerState(state);
    }

    private void Update()
    {
        if(photonView.IsMine)
        {
            Move();

            AnimatorUpdate();
        }
        
    }

    private void Move()
    {
        inputVec = joystick.inputVec;

        rigid.velocity = inputVec*moveSpeed;
        
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
        anim.SetTrigger("isDeath");
        SetPlayerState(PlayerState.Ghost);
        //Saebom.PlayGameManager.Instance.PlayerDie;
    }

    private void SetNamePosition()
    {
        nameTransform.anchoredPosition = namePosition;
    }

    public void OnInactive()
    {
        
        anim.SetTrigger("IsInactive");
        SetPlayerState(PlayerState.Inactive);

    }

    public void OnActive()
    {
        
        anim.SetTrigger("IsActive");
        SetPlayerState(PlayerState.Active);
    }

    private void SetPlayerState(PlayerState playerState)
    {
        state = playerState;
        switch(state)
        {
            case PlayerState.Active:
                SetLayer(LayerMask.NameToLayer("Player"));
                cullingMask.OffLayerMask(LayerMask.NameToLayer("InActive"));
                cullingMask.OffLayerMask(LayerMask.NameToLayer("Ghost"));
                cullingMask.OffLayerMask(LayerMask.NameToLayer("Shadow"));

                break;
            case PlayerState.Inactive:
                SetLayer(LayerMask.NameToLayer("InActive"));
                cullingMask.OnLayerMask(LayerMask.NameToLayer("InActive"));
                break;
            case PlayerState.Ghost:
                SetLayer(LayerMask.NameToLayer("Ghost"));
                colli.enabled = false;
                SetNamePosition();
                cullingMask.OnLayerMask(LayerMask.NameToLayer("Ghost"));
                cullingMask.OnLayerMask(LayerMask.NameToLayer("Shadow"));
                cullingMask.OnLayerMask(LayerMask.NameToLayer("InActive"));
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Saebom.MissionButton.Instance.inter = collision.GetComponent<InterActionAdapter>();
        Saebom.MissionButton.Instance.MissionButtonOn();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        Saebom.MissionButton.Instance.MissionButtonOff();

    }
}
