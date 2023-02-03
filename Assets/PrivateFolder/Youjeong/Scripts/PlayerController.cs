using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;

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

    private void Awake()
    {
        spriteRenderer=GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody2D>();
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


    /* private void OnTriggerEnter2D(Collider2D collision)
     {
         Saebom.MissionButton.Instance.inter = collision.GetComponent<InterActionAdapter>();
         Saebom.MissionButton.Instance.MissionButtonOn();
     }

     private void OnTriggerExit2D(Collider2D collision)
     {

         Saebom.MissionButton.Instance.MissionButtonOff();

     }*/
}
