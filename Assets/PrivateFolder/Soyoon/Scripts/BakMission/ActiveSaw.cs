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
        private float sawingBound; // ���� �ڸ��ٰ� �ľ��ϱ� ���� bound

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
            //Debug.Log(string.Format("�ڸ��� �ӵ� : {0}", Mathf.Abs(Input.GetAxis("Mouse Y"))));

            // ���� �������� �ʾƵ� ����ٰ� �ö󰡴� ��츦 ���� ���� 0.1f���� Ŭ ��츸 ���� �ڸ��� ���̶�� �Ǵ�
            if (Mathf.Abs(Input.GetAxis("Mouse Y")) > sawingBound && !isSawing) 
            {
                if (Input.GetAxis("Mouse Y") > sawingBound)
                { 
                    saw1.Play();
                    saw2.Stop();
                }// TODO : �ö󰡴� �Ҹ� ���
                else
                {
                    saw1.Stop();
                    saw2.Play(); 
                }// TODO : �������� �Ҹ� ���

                isSawing = true;
                // ���� �ڸ��� ���̶�� �Ǵ�
                HashTable props = new HashTable();
                props.Add("IsSawing", true);
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            }
            else if(Mathf.Abs(Input.GetAxis("Mouse Y")) <= sawingBound && isSawing)
            {
                // TODO : �Ҹ� ����
                saw1.Stop();
                saw2.Stop();
                isSawing = false;
                // �� �ڸ��⸦ ���ƴٰ� �Ǵ�
                HashTable props = new HashTable();
                props.Add("IsSawing", false);
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            }
        }
    }
}
