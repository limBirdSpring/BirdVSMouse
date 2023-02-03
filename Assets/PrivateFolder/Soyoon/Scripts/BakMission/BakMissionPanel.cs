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

        public void OnPointerDown(PointerEventData eventData)
        {
            OnBeginSaw?.Invoke();

            HashTable props = new HashTable();
            props.Add("isBakMission", true);
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnEndSaw?.Invoke();

            HashTable props = new HashTable();
            props.Add("isBakMission", false);
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
    }
}
