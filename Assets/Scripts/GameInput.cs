using UnityEngine;

// Wrapper de InputActions

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private InputActions inputActions;
    private void Awake()
    {
        inputActions = new InputActions();
        Instance = this;
        this.inputActions.Enable();
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
    public Vector2 getMovementInputVector2()
    {
        return inputActions.Player.MovementPad.ReadValue<Vector2>();
    }
    private void OnDestroy()
    {
        this.inputActions.Disable();
    }
}
