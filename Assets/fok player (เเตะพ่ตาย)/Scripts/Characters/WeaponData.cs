using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("กระสุน")]
    public int maxAmmo;          // กระสุนสูงสุด
    public int ammoPerPickup;    // เก็บได้ครั้งละเท่าไหร่

    public string weaponName;

    public enum WeaponType { Gun, Shotgun, MachineGun, Katana }
    public WeaponType weaponType;

    public int damage;
    public float fireRate;      // วินาทีต่อนัด
    public float bulletSpeed;
    public int bulletCount;     // จำนวนกระสุนต่อครั้ง (ลูกซอง = 5+)
    public float spreadAngle;   // องศากระจาย (ลูกซอง)
    public float attackRange;   // ระยะโจมตี (มีด/คาตานะ)

    public GameObject bulletPrefab;
    public GameObject weaponModelPrefab;  // Prefab อาวุธที่จะ Spawn ติดมือ
}