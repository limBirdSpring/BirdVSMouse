using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum RopeState
{
    None,
    Rot,
    Normal
}

public class RopeGame : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image goodRope;
    [SerializeField]
    private Image badRope;
    [SerializeField]
    private Image dropPoint;

    [Header("Audio Source")]
    [SerializeField]
    private AudioSource goodRopeAudioSource;
    [SerializeField]
    private AudioSource rotRopeAudioSource;

    public RopeState curState = RopeState.None;

    private bool itemIsOn;

    private void Start()
    {
        dropPoint.gameObject.SetActive(false);
        itemIsOn = false;
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    public void InstallRope(RopeState state)
    {
        if (state == RopeState.Normal)
        {
            curState = state;
            UpdateUI();
            itemIsOn = true;
        }
        else
        {
            curState = state;
            UpdateUI();
            itemIsOn = true;
        }
    }

    public void UpdateUI()
    {
        if (curState == RopeState.None)
        {
            goodRope.gameObject.SetActive(false);
            badRope.gameObject.SetActive(false);
        }
        // ¾È½â¾úÀ¸¸é
        else if (curState == RopeState.Normal)
        {
            goodRope.gameObject.SetActive(true);
            badRope.gameObject.SetActive(false);
        }
        // ½â¾úÀ¸¸é
        else
        {
            goodRope.gameObject.SetActive(false);
            badRope.gameObject.SetActive(true);
        }
    }

    public void RopeReset()
    {
        goodRope.gameObject.SetActive(false);
        badRope.gameObject.SetActive(false);
        itemIsOn = false;
        curState = RopeState.None;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (Inventory.Instance.isItemSet("RotRope"))
        {
            InstallRope(RopeState.Rot);
            rotRopeAudioSource.Play();
            Inventory.Instance.DeleteItem();
        }
        else if (Inventory.Instance.isItemSet("Rope"))
        {
            InstallRope(RopeState.Normal);
            goodRopeAudioSource.Play();
            Inventory.Instance.DeleteItem();
        }
        else
        {
            return;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!itemIsOn)
            dropPoint.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        dropPoint.gameObject.SetActive(false);
    }
}
