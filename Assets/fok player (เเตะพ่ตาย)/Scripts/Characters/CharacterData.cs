using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite portrait;
    public int maxHealth;
    public float moveSpeed;
    public float dashDistance;
    public float dashCooldown;

    public WeaponData weapon;
    public GameObject hatPrefab;
}