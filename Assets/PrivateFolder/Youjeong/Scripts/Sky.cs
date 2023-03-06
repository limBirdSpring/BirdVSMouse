using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform startPosition;
    [SerializeField]
    private Transform goalPosition; 

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.right * speed*Time.deltaTime);
        if (transform.position.x >= goalPosition.position.x)
            transform.position = startPosition.position;
    }
}
