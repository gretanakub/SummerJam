using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool ShootInput { get; private set; }
    public bool ShootInputDown { get; private set; }
    public bool DashInputDown { get; private set; }
    public bool isGamepad { get; private set; }

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction shootAction;
    private InputAction dashAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        shootAction = playerInput.actions["Shoot"];
        dashAction = playerInput.actions["Dash"];
    }

    void Update()
    {
        MoveInput = moveAction.ReadValue<Vector2>();
        LookInput = lookAction.ReadValue<Vector2>();
        ShootInput = shootAction.IsPressed();
        ShootInputDown = shootAction.WasPressedThisFrame();
        DashInputDown = dashAction.WasPressedThisFrame();
        isGamepad = playerInput.currentControlScheme == "Gamepad";
    }
}