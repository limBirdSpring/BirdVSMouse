using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (null == instance)
            {
                GameObject gameObject = GameObject.FindObjectOfType<T>().gameObject;
                if (null == gameObject)
                {
                    gameObject = new GameObject();
                    gameObject.AddComponent<T>();
                }
                instance = gameObject.GetOrAddComponent<T>();

                //DontDestroyOnLoad(gameObject);  // 씬이 변경되어도 제거되지 않는 게임오브젝트로 설정
            }
            return instance;
        }
    }
}