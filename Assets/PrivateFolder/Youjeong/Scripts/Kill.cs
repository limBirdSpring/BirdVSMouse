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

    private bool isBrid;
    private GameObject Black;
    private Animator anim;   
    private float timeReveal=0;

    private void Start()
    {
       /* Me.sprite = PlayGameManager.Instance.myPlayerState.sprite;
        Me.SetNativeSize();
        Spy.SetNativeSize();*/
        anim = Me.GetComponent<Animator>();
        isBrid = PlayGameManager.Instance.myPlayerState.isBird;
        FindSpy(isBrid);
    }

    private void FindSpy(bool isBird)
    {
        if (isBird)
            Black = BlackMouse;
        else
            Black = BlackBird;
        /*foreach (Saebom.PlayerState player in PlayGameManager.Instance.playerList)
        {
            if (player.isSpy && isBird)
                Spy.sprite = player.sprite;
        }*/
    }

    private void FixedUpdate()
    {
        timeReveal += Time.deltaTime*0.1f;
        if (timeReveal == 0.4f)
        {
            Black.SetActive(true);
        }
        if ( timeReveal >0.4f&&timeReveal < 0.9f)
        {
            Vector3 dir = new Vector3(timeReveal - 1.5f, 0.5f - timeReveal, 0).normalized;
            Spy.transform.transform.up = dir;
            Spy.transform.Translate(dir * speed, Space.World);
        }
        else if (timeReveal == 0.9)
        {
            anim.SetTrigger("IsDie");
        }
        else if (timeReveal >= 2)
            this.gameObject.SetActive(false);
    }
}
