using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public enum SunOrMoonState
{
    None,
    Down,
    Right,
    Left
}

public enum Identity
{
    Sun,
    Moon
}

public class SunOrMoon : MonoBehaviour
{
    [SerializeField]
    private Image sun;
    [SerializeField]
    private Image moon;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Transform originalPos;

    private SunOrMoonState curState;
    private Coroutine moveCoroutine;

    [SerializeField]
    private Identity identity;

    private bool missionSuccess = false;

    [SerializeField]
    private RopeController controller;


    public bool MissionSuccess
    {
        get { return missionSuccess; }
    }

    private void Start()
    {
        curState = SunOrMoonState.None;
    }

    private void FixedUpdate()
    {
        switch (curState)
        {
            case SunOrMoonState.None:
                transform.Translate(Vector2.zero * moveSpeed);
                break;
            case SunOrMoonState.Down:
                transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
                break;
            case SunOrMoonState.Right:
                transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
                break;
            case SunOrMoonState.Left:
                transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
                break;
            default: 
                break;
        }
    }

    public void StartMove()
    {
        curState = SunOrMoonState.Down;
    }

    public void ResetPos()
    {
        this.gameObject.transform.position = originalPos.transform.position;
        curState = SunOrMoonState.None;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Left"))
        {
            StartCoroutine(DelayChange("Left"));
        }

        if (collision.gameObject.name.Equals("Right"))
        {
            StartCoroutine(DelayChange("Right"));
        }

        if (collision.gameObject.name.Equals("ArriveSun"))
        {
            curState = SunOrMoonState.None;

            if (identity == Identity.Sun)
            {
                Debug.Log("固记 己傍");
                // TODO: 固记 己傍
                missionSuccess = true;
            }
            else
            {
                missionSuccess = false;
            }
            controller.isArrive = true;
        }
        
        if (collision.gameObject.name.Equals("ArriveMoon"))
        {
            curState = SunOrMoonState.None;
            if (identity == Identity.Moon)
            {
                // TODO: 固记 己傍
                missionSuccess = true;
            }
            else
            {
                missionSuccess = false;
            }
            controller.isArrive = true;
        }

        if (collision.gameObject.name.Equals("Fail"))
        {
            curState = SunOrMoonState.None;
            missionSuccess = false;
            controller.isArrive = true;
        }
    }

    public IEnumerator DelayChange(string dir)
    {
        yield return new WaitForSeconds(0.01f);

        if (dir == "Left")
        {
            if (curState == SunOrMoonState.Down)
            {
                curState = SunOrMoonState.Right;
            }
            else
            {
                curState = SunOrMoonState.Down;
            }
        }
        else
        {
            if (curState == SunOrMoonState.Down)
            {
                curState = SunOrMoonState.Left;
            }
            else
            {
                curState = SunOrMoonState.Down;
            }
        }
    }
}
