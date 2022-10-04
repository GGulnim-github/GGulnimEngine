using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCameraController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Vector2 Look;

    private void Awake()
    {
        Look = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Look = eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Look = Vector2.zero;
    }
}
