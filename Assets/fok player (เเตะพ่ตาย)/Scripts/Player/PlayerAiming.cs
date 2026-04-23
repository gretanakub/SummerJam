using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : MonoBehaviour
{
    public LayerMask groundLayer;

    void Update()
    {
        // เช็คว่ากำลังใช้ Gamepad อยู่ไหม
        if (Gamepad.current != null && Gamepad.current.rightStick.ReadValue().magnitude > 0.1f)
        {
            RotateWithGamepad();
        }
        else
        {
            RotateTowardsMouse();
        }
    }

    void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPoint = hit.point;
            Vector3 direction = targetPoint - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }

    void RotateWithGamepad()
    {
        // อ่านค่า Right Stick จาก Gamepad
        Vector2 stick = Gamepad.current.rightStick.ReadValue();
        Vector3 direction = new Vector3(stick.x, 0f, stick.y);

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}