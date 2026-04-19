using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    public event Action<ClearCounter> OnSelectedCounterChanged;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float playerRadius = 0.5f;
    [SerializeField] private float gamepadLookSensitivity = 5f;
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask counterLayerMask;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;
    private Camera mainCamera;
    private bool isGamepad = false;
    private Vector3 lastGamepadLookDir = Vector3.forward;
    private ClearCounter selectedCounter;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (context.phase != InputActionPhase.Canceled)
            isGamepad = context.control.device is Gamepad;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (selectedCounter != null)
                selectedCounter.Interact();
        }
    }

    public ClearCounter GetSelectedCounter()
    {
        return selectedCounter;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleInteract();
    }

    private void HandleMovement()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);
        float moveDistance = speed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(
            transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius,
            moveDir,
            moveDistance
        );

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(
                transform.position,
                transform.position + Vector3.up * playerHeight,
                playerRadius,
                moveDirX,
                moveDistance
            );

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(
                    transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius,
                    moveDirZ,
                    moveDistance
                );

                if (canMove)
                    moveDir = moveDirZ;
            }
        }

        if (canMove)
            controller.Move(moveDir * moveDistance);

        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleRotation()
    {
        if (isGamepad)
        {
            Vector2 rightStick = Gamepad.current != null
                ? Gamepad.current.rightStick.ReadValue()
                : Vector2.zero;

            if (rightStick.magnitude > 0.1f)
                lastGamepadLookDir = new Vector3(rightStick.x, 0, rightStick.y).normalized;

            transform.forward = Vector3.Slerp(
                transform.forward,
                lastGamepadLookDir,
                Time.deltaTime * rotateSpeed * gamepadLookSensitivity
            );
        }
        else
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 lookDir = hit.point - transform.position;
                lookDir.y = 0;

                if (lookDir != Vector3.zero)
                {
                    transform.forward = Vector3.Slerp(
                        transform.forward,
                        lookDir.normalized,
                        Time.deltaTime * rotateSpeed
                    );
                }
            }
        }
    }

    private void HandleInteract()
    {
        if (Physics.Raycast(
            transform.position,
            transform.forward,
            out RaycastHit hit,
            interactDistance,
            counterLayerMask))
        {
            if (hit.transform.TryGetComponent(out ClearCounter counter))
            {
                if (counter != selectedCounter)
                {
                    selectedCounter = counter;
                    OnSelectedCounterChanged?.Invoke(selectedCounter);
                }
            }
            else
            {
                SetSelectedCounterNull();
            }
        }
        else
        {
            SetSelectedCounterNull();
        }
    }

    private void SetSelectedCounterNull()
    {
        if (selectedCounter != null)
        {
            selectedCounter = null;
            OnSelectedCounterChanged?.Invoke(null);
        }
    }
}