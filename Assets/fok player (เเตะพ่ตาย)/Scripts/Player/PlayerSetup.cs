using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public Transform hatPoint;
    public Transform handPoint;
    public CharacterData defaultCharacter; // ใส่ตัวละคร default ไว้เผื่อ test

    void Start()
    {
        CharacterData data = null;

        if (CharacterSelector.Instance != null && CharacterSelector.Instance.selectedCharacter != null)
            data = CharacterSelector.Instance.selectedCharacter;
        else
            data = defaultCharacter; // ใช้ default แทนถ้าไม่มีการเลือก

        if (data == null) return;

        GetComponent<HealthSystem>().maxHealth = data.maxHealth;

        PlayerDash dash = GetComponent<PlayerDash>();
        dash.dashSpeed = data.dashDistance;
        dash.dashCooldown = data.dashCooldown;

        GetComponent<WeaponSystem>().SetWeapon(data.weapon);

        if (data.hatPrefab != null)
            Instantiate(data.hatPrefab, hatPoint);

        if (data.weapon.weaponModelPrefab != null)
            Instantiate(data.weapon.weaponModelPrefab, handPoint);
    }
}