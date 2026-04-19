using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : MonoBehaviour
{
    public LayerMask groundLayer;

    private PlayerInputHandler inputHandler;

    void Start()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    void Update()
    {
        if (inputHandler.isGamepad)
            RotateWithGamepad();
        else
            RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(inputHandler.LookInput);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 direction = hit.point - transform.position;
            direction.y = 0f;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void RotateWithGamepad()
    {
        Vector2 stick = inputHandler.LookInput;
        Vector3 direction = new Vector3(stick.x, 0f, stick.y);
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }
}