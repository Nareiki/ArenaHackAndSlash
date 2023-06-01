using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, CharacterControlls.IPlayerInputActions
{
    Vector2 lookVal;
    private CharacterControlls controls;
    [HideInInspector] public Vector2 MoveVal { get; private set; }
    [HideInInspector] public static bool DisableMovement = true;
    [HideInInspector] public event Action OnJumpPerformed;
    [HideInInspector] public event Action OnRunPerformed;

    private void OnEnable()
    {
        if (controls != null)
            return;

        controls = new CharacterControlls();
        controls.PlayerInput.SetCallbacks(this);
        controls.PlayerInput.Enable();
    }

    private void OnDisable()
    {
        controls.PlayerInput.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MoveVal = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookVal = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed || !DisableMovement)
            return;

        OnJumpPerformed?.Invoke();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnRunPerformed?.Invoke();

    }

    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        //GameManager.Instance.OpenMenuScene();
    }

    public void OnOpenCheckpointMenu(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        //GameManager.Instance.OpenCheckpointInventory();
    }
}
