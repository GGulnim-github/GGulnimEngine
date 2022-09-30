using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCameraController : MonoBehaviour, IDragHandler
{
    public PlayerCharacterController characterController;

    public void OnDrag(PointerEventData eventData)
    {
        characterController.CameraRotation();
    }
}
