using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, PlayerControls.IPlayerInputActions
{
    public float MovementX { get; private set; }
    public float MovementY { get; private set; }

    public float MouseX { get; private set; }
    public float MouseY { get; private set; }

    public Action OnPlayerMove;
    public Action OnPlayerStop;
    public Action OnMouseMovementEvent;

    public Action OnInteraction;
    public Action OnOpenInventory;

    public Action OnSprintStart;
    public Action OnSprintStop;

    public PlayerControls controllers;

    private void Start()
    {
        controllers = new PlayerControls();
        controllers.PlayerInput.SetCallbacks(this);
        controllers.PlayerInput.Enable();
    }

    private void OnDestroy()
    {
        controllers.PlayerInput.Disable();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnInteraction?.Invoke();
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnOpenInventory?.Invoke();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            MovementX = context.ReadValue<Vector2>().x;
            MovementY = context.ReadValue<Vector2>().y;
            OnPlayerMove?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            MovementX = 0;
            MovementY = 0;
            OnPlayerStop?.Invoke();
        }
    }

    public void OnMouseMovement(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();

        MouseX = mouseDelta.x;
        MouseY = mouseDelta.y;

        OnMouseMovementEvent?.Invoke();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnSprintStart?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            OnSprintStop?.Invoke();
        }
    }
}
