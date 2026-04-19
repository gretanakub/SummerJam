using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSystem : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform firePoint;

    private float nextFireTime = 0f;

    void Update()
    {
        bool isShooting = false;
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            isShooting = true;
        if (Gamepad.current != null && Gamepad.current.rightTrigger.isPressed)
            isShooting = true;

        if (weaponData.weaponType == WeaponData.WeaponType.Katana)
        {
            // คาตานะกดค้างไม่ได้ ต้องกดใหม่ทุกครั้ง
            if (Mouse.current.leftButton.wasPressedThisFrame && CanFire())
                MeleeAttack();
        }
        else
        {
            if (isShooting && CanFire())
                Shoot();
        }
    }

    bool CanFire()
    {
        return Time.time >= nextFireTime;
    }

void Shoot()
{
    nextFireTime = Time.time + weaponData.fireRate;

    // เพิ่มเสียงยิง
    if (SoundManager.Instance != null)
        SoundManager.Instance.PlayShoot(weaponData.weaponType);

    for (int i = 0; i < weaponData.bulletCount; i++)
    {
        float angle = Random.Range(-weaponData.spreadAngle / 2f, weaponData.spreadAngle / 2f);
        Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, angle, 0);

        GameObject bullet = Instantiate(weaponData.bulletPrefab, firePoint.position, rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = rotation * Vector3.forward * weaponData.bulletSpeed;
    }

    nextFireTime = Time.time + weaponData.fireRate;
    Debug.Log("FireRate: " + weaponData.fireRate + " | NextFireTime: " + nextFireTime);
    // ...
}

void MeleeAttack()
{
    nextFireTime = Time.time + weaponData.fireRate;

    // เพิ่มเสียงคาตานะ
    if (SoundManager.Instance != null)
        SoundManager.Instance.PlayShoot(weaponData.weaponType);

    Collider[] hits = Physics.OverlapSphere(firePoint.position, weaponData.attackRange);
    foreach (Collider hit in hits)
    {
        if (hit.CompareTag("Enemy"))
        {
            EnemyAiTutorial enemy = hit.GetComponent<EnemyAiTutorial>();
            if (enemy != null)
                enemy.TakeDamage(weaponData.damage);
        }
    }
}

    // เรียกตอนโหลดตัวละคร
    public void SetWeapon(WeaponData data)
    {
        weaponData = data;
    }
}