using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public Transform handPoint;
    public CharacterData defaultCharacter;
    public int playerIndex = 0;

    void Start()
    {
        CharacterData data = null;

        if (CharacterSelector.Instance != null)
            data = CharacterSelector.Instance.GetCharacterForPlayer(playerIndex);

        if (data == null)
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
    }
}