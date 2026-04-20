using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("เสียงยิง")]
    public AudioClip shootGun;
    public AudioClip shootMachineGun;
    public AudioClip shootShotgun;
    public AudioClip shootKatana;

    [Header("เสียง Reload")]
    public AudioClip reloadRevolver;
    public AudioClip reloadMachineGun;
    public AudioClip reloadShotgun;

    [Header("เสียงโดนดาเมจ")]
    public AudioClip playerHit;
    public AudioClip enemyHit;
    public AudioClip playerDeath;
    public AudioClip enemyDeath;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayShoot(WeaponData.WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponData.WeaponType.Gun:
                if (shootGun != null) audioSource.PlayOneShot(shootGun);
                break;
            case WeaponData.WeaponType.MachineGun:
                if (shootMachineGun != null) audioSource.PlayOneShot(shootMachineGun);
                break;
            case WeaponData.WeaponType.Shotgun:
                if (shootShotgun != null) audioSource.PlayOneShot(shootShotgun);
                break;
            case WeaponData.WeaponType.Katana:
                if (shootKatana != null) audioSource.PlayOneShot(shootKatana);
                break;
        }
    }

    public void PlayReload(WeaponData.WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponData.WeaponType.Gun:
                if (reloadRevolver != null) audioSource.PlayOneShot(reloadRevolver);
                break;
            case WeaponData.WeaponType.MachineGun:
                if (reloadMachineGun != null) audioSource.PlayOneShot(reloadMachineGun);
                break;
            case WeaponData.WeaponType.Shotgun:
                if (reloadShotgun != null) audioSource.PlayOneShot(reloadShotgun);
                break;
        }
    }

    public void PlayPlayerHit() => PlaySound(playerHit);
    public void PlayEnemyHit() => PlaySound(enemyHit);
    public void PlayPlayerDeath() => PlaySound(playerDeath);
    public void PlayEnemyDeath() => PlaySound(enemyDeath);

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}