using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Saebom;

namespace Youjeong
{
    public class Frog : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private HagnariGame game;
        [SerializeField]
        private HangariManager manager;
        [SerializeField]
        private GameObject sprayWater;
        [SerializeField]
        private RectTransform outTransform;

        [Header("Audio Source")]
        [SerializeField]
        private AudioSource sprayAudioSource;
        [SerializeField]
        private AudioSource inFrogAudioSource;
        [SerializeField]
        private AudioSource outFrogAudioSource;

        private float delay;
        private RectTransform rectTransform;
        private bool isOutHangari = false;
        private Coroutine spray;
        private Vector3 originPosition;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            originPosition = rectTransform.position;
            delay = game.delay;
        }
       
        private void OnDisable()
        {
             Reset();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(!isOutHangari&&manager.waterAmount>0)
            {
                isOutHangari = true;
                game.MinusWater();
                sprayWater.SetActive(true);
                spray = StartCoroutine("SprayWaterCoroutine");
                OutFrog();
                sprayAudioSource.Play();
            }
        }

        public void OutFrog()
        {
            outFrogAudioSource.Play();
            rectTransform.position = outTransform.position;
        }

        public void InFrog()
        {
            inFrogAudioSource.Play();
            rectTransform.position = originPosition;
        }

        private IEnumerator SprayWaterCoroutine()
        {
            yield return new WaitForSeconds(delay);
            Reset();

        }

        private void Reset()
        {
            InFrog();
            if (spray != null)
                StopCoroutine(spray);
            isOutHangari = false;
            sprayWater.SetActive(false);
        }

    }
}

