using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;

public enum PlayerState { Active, Inactive, Ghost}

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    [SerializeField]
    private Joystick joystick;
    [SerializeField]
    private float moveSpeed=10;
    [SerializeField]
    private GameObject death;
    [SerializeField]
    private Collider2D colli;

    private Vector2 inputVec;
    public PlayerState state { get; private set; } = PlayerState.Active;

    private void Awake()
    {
        spriteRenderer=GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        SetPlayerState(PlayerState.Active);
    }

    private void Update()
    {
        Move();

        AnimatorUpdate();
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
        Instantiate(death, transform.position, death.transform.rotation);
        anim.SetTrigger("isDeath");
        SetPlayerState(PlayerState.Ghost);
        colli.enabled = false; // 콜라이더 비활성화
    }

    public void OnInactive()
    {
        //TODO :  비활성화 시간대에 할일 넣기
        anim.SetTrigger("IsInactive");
        SetPlayerState(PlayerState.Inactive);

    }

    public void OnActive()
    {
        //TODO :  활성화 시간대에 할일 넣기
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
                break;
            case PlayerState.Inactive:
                SetLayer(LayerMask.NameToLayer("Ghost"));
                break;
            case PlayerState.Ghost:
                SetLayer(LayerMask.NameToLayer("Ghost"));
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
