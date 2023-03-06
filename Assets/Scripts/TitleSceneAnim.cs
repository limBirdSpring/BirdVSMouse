using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneAnim : MonoBehaviour
{
    [SerializeField]
    private Animator bird;

    [SerializeField]
    private Animator mouse;

    private void OnEnable()
    {
        bird.SetTrigger("Move");
        mouse.enabled = true;
    }
}
