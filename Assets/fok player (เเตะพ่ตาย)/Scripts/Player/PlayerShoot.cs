using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;

    private float nextFireTime = 0f;

    void Update()
    {
        bool isShooting = false;

        // Mouse คลิกซ้าย
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            isShooting = true;

        // Gamepad กด R2 / RT
        if (Gamepad.current != null && Gamepad.current.rightTrigger.isPressed)
            isShooting = true;

        if (isShooting && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}