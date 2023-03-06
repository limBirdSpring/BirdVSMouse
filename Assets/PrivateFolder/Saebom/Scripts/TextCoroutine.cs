using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextCoroutine : MonoBehaviour
{
    [SerializeField]
    private float time = 1.2f;

    private void OnEnable()
    {
        StartCoroutine(Cor());
    }

    private IEnumerator Cor()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
