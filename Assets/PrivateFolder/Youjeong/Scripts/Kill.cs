using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;
using UnityEngine.UI;

public class Kill : MonoBehaviour
{
    [SerializeField]
    private GameObject BlackBird;
    [SerializeField]
    private GameObject BlackMouse;
    [SerializeField]
    private Image Spy;
    [SerializeField]
    private Image Me;
    [SerializeField]
    private float speed;

    [Header("AudioSource")]
    [SerializeField]
    private AudioSource punch1;
    [SerializeField]
    private AudioSource punch2;

    private bool isBrid;
    private GameObject Black;
    private Animator anim;   
    private float timeReveal=0;
    private float time = 0;

    private bool isReveal=false;
    private bool isDie = false;

    private void Start()
    {
        Me.sprite = PlayGameManager.Instance.myPlayerState.sprite;
        Me.SetNativeSize();
        anim = Me.GetComponent<Animator>();
        isBrid = true;
        isBrid = PlayGameManager.Instance.myPlayerState.isBird;
        FindSpy(isBrid);
        Spy.SetNativeSize();
    }

    private void FindSpy(bool isBird)
    {
        if (isBird)
            Black = BlackMouse;
        else
            Black = BlackBird;
        foreach (Saebom.PlayerState player in PlayGameManager.Instance.playerList)
        {
            if (player.isSpy && player.isBird && isBird)
                Spy.sprite = player.sprite;
            else if (player.isSpy && !player.isBird && !isBird)
                Spy.sprite = player.sprite;
        }
    }

    private void FixedUpdate()
    {
        timeReveal += Time.deltaTime;
        if (timeReveal >= 0.7f&&!isReveal)
        {
            Black.SetActive(true);
            punch1.Play();
            isReveal = true;
        }
        if ( timeReveal >0.7f&&timeReveal < 3.7f)
        {
            time += Time.deltaTime;
            Vector3 dir = new Vector3(time - 3.0f, 1.5f - time, 0).normalized;
            Spy.transform.transform.up = dir;
            Spy.transform.Translate(dir * speed, Space.World);
        }
        if (timeReveal >= 1.2f&&!isDie)
        {
            punch2.Play();
            anim.SetTrigger("IsDie");
            isDie = true;
        }
        else if (timeReveal >= 2.2f)
            this.transform.parent.gameObject.SetActive(false);
    }
}
