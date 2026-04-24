using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : MonoBehaviour
{
    private Transform modelPoint;
    private Plane groundPlane;

    void Start()
    {
        modelPoint = transform.Find("ModelPoint");
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    void Update()
    {
        if (modelPoint == null)
            modelPoint = transform.Find("ModelPoint");

        if (Gamepad.current != null && Gamepad.current.rightStick.ReadValue().magnitude > 0.1f)
            RotateWithGamepad();
        else
            RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            Vector3 direction = hitPoint - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero && modelPoint != null)
                modelPoint.rotation = Quaternion.LookRotation(direction);
        }
    }

    void RotateWithGamepad()
    {
        Vector2 stick = Gamepad.current.rightStick.ReadValue();
        Vector3 direction = new Vector3(stick.x, 0f, stick.y);

        if (direction != Vector3.zero && modelPoint != null)
            modelPoint.rotation = Quaternion.LookRotation(direction);
    }
}