using UnityEngine;
using UnityEngine.AI;

public class EnemyAiTutorial : MonoBehaviour
{
    public EnemyData enemyData;

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public GameObject projectile;
    public GameObject ammoDropPrefab;
    public GameObject fruitDropPrefab;

    private float health;
    private bool alreadyAttacked;
    private bool walkPointSet;
    private Vector3 walkPoint;
    private bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (enemyData != null)
        {
            health = enemyData.maxHealth;
            agent.speed = enemyData.moveSpeed;
        }

        HealthSystem hp = GetComponent<HealthSystem>();
        if (hp != null)
            hp.onDeath.AddListener(DropItems);
    }

    private void Update()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                return; // หาไม่เจอค่อย return
        }

        float sightRange = enemyData != null ? enemyData.sightRange : 10f;
        float attackRange = enemyData != null ? enemyData.attackRange : 3f;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        if ((transform.position - walkPoint).magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float range = enemyData != null ? enemyData.sightRange : 10f;
        float randomZ = Random.Range(-range, range);
        float randomX = Random.Range(-range, range);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Vector3 spawnPos = transform.position + transform.forward * 1.5f;
            Rigidbody rb = Instantiate(projectile, spawnPos, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            alreadyAttacked = true;
            float cooldown = enemyData != null ? enemyData.attackCooldown : 1f;
            Invoke(nameof(ResetAttack), cooldown);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayEnemyHit();

        if (health <= 0)
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayEnemyDeath();
        }
    }

    private void DropItems()
    {
        float ammoChance = enemyData != null ? enemyData.ammoDropChance : 0.5f;
        float fruitChance = enemyData != null ? enemyData.fruitDropChance : 0.3f;

        if (ammoDropPrefab != null && Random.value <= ammoChance)
            Instantiate(ammoDropPrefab, transform.position, Quaternion.identity);

        if (fruitDropPrefab != null && Random.value <= fruitChance)
            Instantiate(fruitDropPrefab, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        if (enemyData == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyData.sightRange);
    }
}