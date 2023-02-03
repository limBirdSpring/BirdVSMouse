using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour,IDragHandler, IPointerDownHandler,IPointerUpHandler
{
    [SerializeField]
    private Vector2 range = new Vector2(80,80);

    public Vector2 inputVec { get; private set; }
    private Vector2 inputDir;
    private Vector2 center;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        center = rectTransform.position;
       
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetInput(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetInput(eventData);
    }

    private void SetInput(PointerEventData eventData)
    {
        inputDir = eventData.position - center;
        inputDir.x = Mathf.Clamp( inputDir.x / range.x,-1,1);
        inputDir.y = Mathf.Clamp(inputDir.y / range.y, -1, 1);
        
        inputVec = inputDir.sqrMagnitude>1?  inputDir.normalized: inputDir;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVec = Vector2.zero;
    }
}
