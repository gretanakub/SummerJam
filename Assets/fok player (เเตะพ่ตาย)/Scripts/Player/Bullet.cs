using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    public float lifetime = 3f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;
        if (other.CompareTag("Bullet")) return;

        // เช็ค HealthSystem ก่อน (สำหรับ Player)
        HealthSystem health = other.GetComponent<HealthSystem>();
        if (health != null)
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // ถ้าไม่มี HealthSystem ให้เช็ค Enemy แทน
        EnemyAiTutorial enemy = other.GetComponent<EnemyAiTutorial>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}