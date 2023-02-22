using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    private float loadTime = 0;

    [SerializeField]
    private Image slider;

    private void Start()
    {
        StartCoroutine(LoadingCoroutine());
    }

    private IEnumerator LoadingCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("LobbyTestScene");
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            loadTime += Time.deltaTime;
            slider.fillAmount = loadTime / 5f;

            if (loadTime > 5f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
