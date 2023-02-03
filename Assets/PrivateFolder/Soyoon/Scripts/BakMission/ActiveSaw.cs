using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using HashTable = ExitGames.Client.Photon.Hashtable;

namespace SoYoon
{
    public class ActiveSaw : MonoBehaviour
    {
        [SerializeField]
        private float minY;
        [SerializeField]
        private float maxY;

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

            // 톱을 움직이지 않아도 진행바가 올라가는 경우를 막기 위해 0.1f보다 클 경우만 박을 자르는 중이라고 판단
            if (Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.1f && !isSawing) 
            {
                isSawing = true;
                // 박을 자르는 중이라고 판단
                HashTable props = new HashTable();
                props.Add("IsSawing", true);
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            }
            else if(Mathf.Abs(Input.GetAxis("Mouse Y")) <= 0.1f && isSawing)
            {
                isSawing = false;
                // 박 자르기를 마쳤다고 판단
                HashTable props = new HashTable();
                props.Add("IsSawing", true);
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            }
        }
    }
}
