using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using HashTable = ExitGames.Client.Photon.Hashtable;

namespace SoYoon
{
    public class BakMissionPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public UnityEvent OnBeginSaw;
        public UnityEvent OnEndSaw;

        private bool isBakMissionComplete;
        private bool isClicked;

        private void Start()
        {
            isBakMissionComplete = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!isBakMissionComplete && !isClicked)
            {
                isClicked = true;
                OnBeginSaw?.Invoke();

                HashTable props = new HashTable();
                props.Add("IsBakMission", true);
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!isBakMissionComplete && isClicked)
            {
                isClicked = false;
                OnEndSaw?.Invoke();

                HashTable props = new HashTable();
                props.Add("IsBakMission", false);
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            }
        }

        public void BakMissionComplete()
        {
            isBakMissionComplete = true;
        }
        public void BakMissionReset()
        {
            isBakMissionComplete = false;
        }
    }
}
