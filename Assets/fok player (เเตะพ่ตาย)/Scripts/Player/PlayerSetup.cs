using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public Transform hatPoint;
    public Transform handPoint;
    public CharacterData defaultCharacter;

    void Start()
    {
        CharacterData data = null;

        if (CharacterSelector.Instance != null && CharacterSelector.Instance.selectedCharacter != null)
            data = CharacterSelector.Instance.selectedCharacter;
        else
            data = defaultCharacter;

        if (data == null) return;

        PlayerHealthSystem hp = GetComponent<PlayerHealthSystem>();
        if (hp != null)
        {
            hp.maxHearts = data.maxHearts;
            hp.currentHearts = data.maxHearts;
        }

        PlayerDash dash = GetComponent<PlayerDash>();
        if (dash != null)
        {
            dash.dashSpeed = data.dashDistance;
            dash.dashCooldown = data.dashCooldown;
        }

        WeaponSystem weapon = GetComponent<WeaponSystem>();
        if (weapon != null)
            weapon.SetWeapon(data.weapon);

        if (data.hatPrefab != null)
            Instantiate(data.hatPrefab, hatPoint);

        if (data.weapon != null && data.weapon.weaponModelPrefab != null)
            Instantiate(data.weapon.weaponModelPrefab, handPoint);
    }
}