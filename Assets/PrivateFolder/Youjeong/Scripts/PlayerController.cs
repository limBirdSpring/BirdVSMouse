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

    private Vector2 inputVec;
    public PlayerState state = PlayerState.Active;

    private void Awake()
    {
        spriteRenderer=GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        SetPlayerState(LayerMask.NameToLayer("Player"));
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
        state = PlayerState.Ghost;
        SetPlayerState(LayerMask.NameToLayer("Ghost"));
    }

    public void OnInactive()
    {
        //TODO :  비활성화 시간대에 할일 넣기
        anim.SetTrigger("IsInactive");

    }

    public void OnActive()
    {
        //TODO :  활성화 시간대에 할일 넣기
        anim.SetTrigger("IsActive");
    }

    private void SetPlayerState(LayerMask layer)
    {
        gameObject.layer = layer;
        foreach(Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = layer;
        }
    }
}
