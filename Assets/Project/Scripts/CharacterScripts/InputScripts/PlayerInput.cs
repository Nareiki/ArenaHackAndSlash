using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, CharacterControlls.IPlayerInputActions
{
    private Vector2 _lookVal;
    private CharacterControlls _controls;
    [HideInInspector] public Vector2 MoveVal { get; private set; }
    [HideInInspector] public static bool DisableMovement = true;
    [HideInInspector] public event Action OnJumpPerformed;
    [HideInInspector] public event Action OnRunPerformed;

    private void OnEnable()
    {
        if (_controls != null)
            return;

        _controls = new CharacterControlls();
        _controls.PlayerInput.SetCallbacks(this);
        _controls.PlayerInput.Enable();
    }

    private void OnDisable()
    {
        _controls.PlayerInput.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MoveVal = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookVal = context.ReadValue<Vector2>();
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
