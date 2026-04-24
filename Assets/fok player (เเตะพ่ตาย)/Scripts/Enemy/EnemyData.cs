using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("General")]
    public string enemyName;
    public GameObject enemyPrefab;

    [Header("Stats")]
    public float maxHealth = 100f;
    public float moveSpeed = 5f;
    public float damage = 10f;

    [Header("Combat")]
    public float attackRange = 3f;
    public float sightRange = 10f;
    public float attackCooldown = 1f;

    [Header("Drop")]
    [Range(0f, 1f)] public float ammoDropChance = 0.5f;
    [Range(0f, 1f)] public float fruitDropChance = 0.3f;
    [Range(0f, 1f)] public float healthBottleDropChance = 0.3f; // ← เพิ่มตรงนี้
    public int ammoDropAmount = 10;
}