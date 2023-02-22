using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingTMP : MonoBehaviour
{
    private TextMeshProUGUI text;

    private int i = 0;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartCoroutine(LoadingTMPCoroutine());
    }

    private IEnumerator LoadingTMPCoroutine()
    {
        yield return new WaitForSeconds(1f);
        if (i < 3)
        {
            text.text += ".";

            i++;
        }
        else
        {
            text.text = "Loading";
            i = 0;
        }
    }
}
