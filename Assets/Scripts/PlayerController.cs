using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour, IKitchenObjectParent
{
    public event Action<ICounter> OnSelectedCounterChanged;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float playerRadius = 0.5f;
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private float dropDistance = 1.5f;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private LayerMask kitchenObjectLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isGamepad = false;
    private ICounter selectedCounter;
    private KitchenObject kitchenObject;
    private WeaponSystem weaponSystem;
    private Transform modelPoint;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        weaponSystem = GetComponent<WeaponSystem>();
        modelPoint = transform.Find("ModelPoint");
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
                selectedCounter.Interact(this);
            else if (!HasKitchenObject())
            {
                TryPickUpFromGround();
                if (HasKitchenObject())
                    weaponSystem?.SetWeaponLocked(true);
            }
        }
    }

    public void InteractAlternate(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (selectedCounter != null && selectedCounter is CuttingCounter cuttingCounter)
                cuttingCounter.InteractAlternate(this);
        }
    }

    public void Drop(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (HasKitchenObject())
            {
                DropKitchenObject();
                weaponSystem?.SetWeaponLocked(false);
            }
        }
    }

    private void TryPickUpFromGround()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            interactDistance,
            kitchenObjectLayerMask
        );

        if (colliders.Length > 0)
        {
            KitchenObject nearest = null;
            float nearestDistance = float.MaxValue;

            foreach (Collider col in colliders)
            {
                if (col.TryGetComponent(out KitchenObject obj))
                {
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
                nearest.SetKitchenObjectParent(this);
        }
    }

    private void DropKitchenObject()
    {
        Vector3 dropPosition = transform.position + transform.forward * dropDistance;

        if (Physics.Raycast(dropPosition + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 5f))
            dropPosition = hit.point + Vector3.up * 0.5f;

        kitchenObject.Drop(dropPosition);
        kitchenObject = null;
    }

    public ICounter GetSelectedCounter() => selectedCounter;
    public Transform GetKitchenObjectFollowTransform() => kitchenObjectHoldPoint;
    public void SetKitchenObject(KitchenObject obj) => kitchenObject = obj;
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void ClearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;

    void Update()
    {
        if (modelPoint == null)
            modelPoint = transform.Find("ModelPoint");

        HandleMovement();
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
                moveDir = moveDirX;
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

    private void HandleInteract()
    {
        if (modelPoint == null) return;

        if (Physics.Raycast(
            transform.position,
            modelPoint.forward,
            out RaycastHit hit,
            interactDistance,
            counterLayerMask))
        {
            if (hit.transform.TryGetComponent(out ICounter counter))
            {
                if (counter != selectedCounter)
                {
                    selectedCounter = counter;
                    OnSelectedCounterChanged?.Invoke(selectedCounter);
                }
            }
            else
                SetSelectedCounterNull();
        }
        else
            SetSelectedCounterNull();
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