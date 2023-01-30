using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CamaraMove_Sample : MonoBehaviour
{
    private Camera cam;

    private float speed = 10;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector2.down * speed * Time.deltaTime);

    }
}
