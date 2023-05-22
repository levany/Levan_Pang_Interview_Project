using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HudInputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Properties

    public bool IsHeld;

    InputActionReference InputAction;
    
    // Events

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        IsHeld = true;
        InputAction.action.Reset();
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        IsHeld = false;
    }


}
