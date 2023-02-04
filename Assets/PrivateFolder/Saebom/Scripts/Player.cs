using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saebom
{
    public class Player : MonoBehaviour
    {

        private float speed = 10;

        private void Update()
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



        private void OnTriggerEnter2D(Collider2D collision)
        {
            Saebom.MissionManager.Instance.inter = collision.GetComponent<InterActionAdapter>();
            Saebom.MissionManager.Instance.MissionButtonOn();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            
            Saebom.MissionManager.Instance.MissionButtonOff();

        }
    }
}