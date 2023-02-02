using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Joystick joystick;
    

    private Vector2 input;

    private void Awake()
    {
        input = joystick.inputVec;
        spriteRenderer=GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        
    }

    private void Move()
    {
        
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        Saebom.MissionButton.Instance.inter = collision.GetComponent<InterActionAdapter>();
        Saebom.MissionButton.Instance.MissionButtonOn();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        Saebom.MissionButton.Instance.MissionButtonOff();

    }*/
}
