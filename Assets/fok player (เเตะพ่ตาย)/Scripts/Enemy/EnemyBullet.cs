using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        // กระสุน Enemy โดน Enemy ด้วยกันไม่ได้
        if (other.CompareTag("Enemy")) return;
        if (other.CompareTag("Bullet")) return;

        // โดน Player
        HealthSystem health = other.GetComponent<HealthSystem>();
        if (health != null)
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}