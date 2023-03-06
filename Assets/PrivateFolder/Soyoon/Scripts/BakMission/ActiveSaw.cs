using Photon.Pun;
using System.Collections;
using UnityEngine;
using HashTable = ExitGames.Client.Photon.Hashtable;

namespace SoYoon
{
    public class ActiveSaw : MonoBehaviour
    {
        [SerializeField]
        private float minY = Screen.height * 0.25f;
        [SerializeField]
        private float maxY = Screen.height * 0.75f;
        [SerializeField]
        private float sawingBound; // 박을 자른다고 파악하기 위한 bound

        [Header("AudioSource")]
        [SerializeField]
        private AudioSource saw1;
        [SerializeField]
        private AudioSource saw2;

        private bool isSawing;

        private void Start()
        {
            isSawing = false;
        }

        private void Update()
        {
            MoveSaw();
        }

        private void MoveSaw()
        {
            Vector2 inputPos = Input.mousePosition;
            float targetY = Mathf.Clamp(inputPos.y, minY, maxY);
            this.transform.localPosition = new Vector2(transform.localPosition.x, targetY);
            //Debug.Log(string.Format("자르는 속도 : {0}", Mathf.Abs(Input.GetAxis("Mouse Y"))));

            // 톱을 움직이지 않아도 진행바가 올라가는 경우를 막기 위해 0.1f보다 클 경우만 박을 자르는 중이라고 판단
            if (Mathf.Abs(Input.GetAxis("Mouse Y")) > sawingBound && !isSawing) 
            {
                if (Input.GetAxis("Mouse Y") > sawingBound)
                { 
                    saw1.Play();
                    saw2.Stop();
                }// TODO : 올라가는 소리 재생
                else
                {
                    saw1.Stop();
                    saw2.Play(); 
                }// TODO : 내려가는 소리 재생

                isSawing = true;
                // 박을 자르는 중이라고 판단
                HashTable props = new HashTable();
                props.Add("IsSawing", true);
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            }
            else if(Mathf.Abs(Input.GetAxis("Mouse Y")) <= sawingBound && isSawing)
            {
                // TODO : 소리 멈춤
                saw1.Stop();
                saw2.Stop();
                isSawing = false;
                // 박 자르기를 마쳤다고 판단
                HashTable props = new HashTable();
                props.Add("IsSawing", false);
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            }
        }
    }
}
