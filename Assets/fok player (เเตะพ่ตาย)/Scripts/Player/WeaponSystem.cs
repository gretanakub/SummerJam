using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;

public class WeaponSystem : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform firePoint;

    public int currentAmmo;
    public int reserveAmmo;
    public bool isReloading = false;
    private bool weaponLocked = false;

    public UnityEvent<int, int> onAmmoChanged;

    private float nextFireTime = 0f;
    private bool shootHeld = false;

    void Awake()
    {
        if (weaponData != null)
        {
            currentAmmo = weaponData.magazineSize;
            reserveAmmo = weaponData.maxAmmo;
        }
    }

    void Update()
    {
        if (weaponData == null) return;
        if (isReloading) return;
        if (weaponLocked) return;

        if (weaponData.weaponType != WeaponData.WeaponType.Katana)
        {
            if (currentAmmo <= 0 && reserveAmmo > 0)
            {
                StartCoroutine(Reload());
                return;
            }

            if (shootHeld && CanFire() && currentAmmo > 0)
                Shoot();
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (weaponData == null) return;
        if (isReloading) return;
        if (weaponLocked) return;

        if (weaponData.weaponType == WeaponData.WeaponType.Katana)
        {
            if (context.phase == InputActionPhase.Started && CanFire())
                MeleeAttack();
        }
        else
        {
            if (context.phase == InputActionPhase.Started)
                shootHeld = true;
            else if (context.phase == InputActionPhase.Canceled)
                shootHeld = false;
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (weaponLocked) return;
        if (context.phase == InputActionPhase.Started)
        {
            if (!isReloading && currentAmmo < weaponData.magazineSize && reserveAmmo > 0)
                StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        shootHeld = false;

        Debug.Log("Reloading...");
        yield return new WaitForSeconds(weaponData.reloadTime);

        int needed = weaponData.magazineSize - currentAmmo;
        int take = Mathf.Min(needed, reserveAmmo);

        currentAmmo += take;
        reserveAmmo -= take;

        onAmmoChanged.Invoke(currentAmmo, reserveAmmo);
        isReloading = false;
    }

    bool CanFire()
    {
        return Time.time >= nextFireTime;
    }

    void Shoot()
    {
        nextFireTime = Time.time + weaponData.fireRate;
        currentAmmo--;

        onAmmoChanged.Invoke(currentAmmo, reserveAmmo);

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
    }

    void MeleeAttack()
    {
        nextFireTime = Time.time + weaponData.fireRate;

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

    public void SetWeapon(WeaponData data)
    {
        weaponData = data;
        currentAmmo = data.magazineSize;
        reserveAmmo = data.maxAmmo;
        onAmmoChanged.Invoke(currentAmmo, reserveAmmo);
    }

    public void AddAmmo(int amount)
    {
        reserveAmmo = Mathf.Clamp(reserveAmmo + amount, 0, weaponData.maxAmmo);
        onAmmoChanged.Invoke(currentAmmo, reserveAmmo);
    }

    public void SetWeaponLocked(bool locked)
    {
        weaponLocked = locked;
        if (locked) shootHeld = false;
    }
}