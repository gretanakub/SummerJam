using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerControllerCooking : MonoBehaviour, IKitchenObjectParent
{
    public event Action<ClearCounter> OnSelectedCounterChanged;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float playerRadius = 0.5f;
    [SerializeField] private float gamepadLookSensitivity = 5f;
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private float dropDistance = 1.5f;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private LayerMask kitchenObjectLayerMask; // Layer ของ KitchenObject
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;
    private Camera mainCamera;
    private bool isGamepad = false;
    private Vector3 lastGamepadLookDir = Vector3.forward;
    private ClearCounter selectedCounter;
    private KitchenObject kitchenObject;

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
            // ถ้ามอง counter อยู่
            if (selectedCounter != null)
            {
                selectedCounter.Interact(this);
            }
            // ถ้าไม่ได้มอง counter → หยิบของจากพื้น
            else if (!HasKitchenObject())
            {
                TryPickUpFromGround();
            }
        }
    }

    private void TryPickUpFromGround()
    {
        // Raycast หา KitchenObject รอบๆ ตัว player
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            interactDistance,
            kitchenObjectLayerMask
        );

        if (colliders.Length > 0)
        {
            // หา KitchenObject ที่ใกล้ที่สุด
            KitchenObject nearest = null;
            float nearestDistance = float.MaxValue;

            foreach (Collider col in colliders)
            {
                if (col.TryGetComponent(out KitchenObject obj))
                {
                    // เช็คว่าของนั้นไม่ได้ถูกถือโดยใครอยู่
                    if (obj.GetKitchenObjectParent() == null)
                    {
                        float dist = Vector3.Distance(transform.position, col.transform.position);
                        if (dist < nearestDistance)
                        {
                            nearestDistance = dist;
                            nearest = obj;
                        }
                    }
                }
            }

            if (nearest != null)
            {
                nearest.SetKitchenObjectParent(this);
            }
        }
    }

    public void Drop(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (HasKitchenObject())
                DropKitchenObject();
        }
    }

    private void DropKitchenObject()
    {
        Vector3 dropPosition = transform.position + transform.forward * dropDistance;

        if (Physics.Raycast(dropPosition + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 5f))
        {
            dropPosition = hit.point + Vector3.up * 0.5f;
        }

        kitchenObject.Drop(dropPosition);
        kitchenObject = null;
    }

    public ClearCounter GetSelectedCounter() => selectedCounter;

    // IKitchenObjectParent
    public Transform GetKitchenObjectFollowTransform() => kitchenObjectHoldPoint;
    public void SetKitchenObject(KitchenObject obj) => kitchenObject = obj;
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void ClearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;

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