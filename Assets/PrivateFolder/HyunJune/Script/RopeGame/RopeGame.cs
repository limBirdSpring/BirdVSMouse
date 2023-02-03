using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RopeGame : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image goodRope;
    [SerializeField]
    private Image badRope;
    [SerializeField]
    private Image dropPoint;

    private bool itemIsOn;

    private void Start()
    {
        goodRope.gameObject.SetActive(false);
        badRope.gameObject.SetActive(false);
        dropPoint.gameObject.SetActive(false);
        itemIsOn = false;
    }

    public void InstallRope(Rope rope)
    {
        if (rope.isRotted != true)
        {
            goodRope.gameObject.SetActive(true);
            itemIsOn = true;
        }
        else
        {
            badRope.gameObject.SetActive(true);
            itemIsOn = true;
        }
    }

    public void RopeReset()
    {
        goodRope.gameObject.SetActive(false);
        badRope.gameObject.SetActive(false);
        itemIsOn = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemData itemData = eventData.pointerDrag.GetComponent<ItemData>();
        if (itemData == null)
            return;

        if (!(itemData is Rope))
            return;

        Rope rope = itemData as Rope;
        InstallRope(rope);
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
