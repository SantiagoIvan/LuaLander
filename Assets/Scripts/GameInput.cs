using UnityEngine;
using System;
// Wrapper de InputActions

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private InputActions inputActions;

    public event EventHandler OnPauseAction;

    private void Awake()
    {
        inputActions = new InputActions();
        Instance = this;
        this.inputActions.Enable();

        this.inputActions.Player.Pause.performed += pause_performed;
    }
    
    private void pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }
    public bool isUpActionPressed()
    {
        return inputActions.Player.LanderUp.IsPressed();
    }
    public bool isRightActionPressed()
    {
        return inputActions.Player.LanderRight.IsPressed();
    }
    public bool isLeftActionPressed()
    {
        return inputActions.Player.LanderLeft.IsPressed();
    }
    public bool isLanderTurboActionPressed()
    {
        return inputActions.Player.LanderTurbo.IsPressed();
    }
    public Vector2 getMovementInputVector2()
    {
        return inputActions.Player.MovementPad.ReadValue<Vector2>();
    }
    private void OnDestroy()
    {
        this.inputActions.Disable();
    }
}
