using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class WeaponSystem : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform firePoint;

    public int currentAmmo;
    public UnityEvent<int, int> onAmmoChanged;

    private float nextFireTime = 0f;
    private PlayerInputHandler inputHandler;

    void Start()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        if (weaponData != null)
            currentAmmo = weaponData.maxAmmo;
    }

    void Update()
    {
        if (weaponData == null) return;

        if (weaponData.weaponType == WeaponData.WeaponType.Katana)
        {
            if (inputHandler.ShootInputDown && CanFire())
                MeleeAttack();
        }
        else
        {
            if (inputHandler.ShootInput && CanFire() && currentAmmo > 0)
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
        currentAmmo--;

        onAmmoChanged.Invoke(currentAmmo, weaponData.maxAmmo);

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
        currentAmmo = data.maxAmmo;
        onAmmoChanged.Invoke(currentAmmo, weaponData.maxAmmo);
    }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, weaponData.maxAmmo);
        onAmmoChanged.Invoke(currentAmmo, weaponData.maxAmmo);
    }
}