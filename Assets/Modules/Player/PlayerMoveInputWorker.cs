using System;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMoveInputWorker
{
    [Inject] private PlayerControls _controls;

    private Action<InputAction.CallbackContext> _action;

    public void Initialize(Action<InputAction.CallbackContext> action)
    {
        _action = action;
        _controls.Player.Movement.performed += _action;
        _controls.Player.Movement.Enable();
    }

    public void Dispose()
    {
        _controls.Player.Movement.performed -= _action;
        _controls.Player.Movement.Disable();        
    }
}

public class PlayerSwapInputWorker
{
    [Inject] private PlayerControls _controls;

    private Action<InputAction.CallbackContext> _action;
    
    public void Initialize(Action<InputAction.CallbackContext> action)
    {
        _action = action;
        _controls.Player.Swap.performed += _action;
        _controls.Player.Swap.Enable();
    }

    public void Dispose()
    {
        _controls.Player.Swap.performed -= _action;
        _controls.Player.Swap.Disable();        
    }
}