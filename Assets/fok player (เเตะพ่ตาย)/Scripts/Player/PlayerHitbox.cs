using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Hitbox โดน: {other.gameObject.name} Tag: {other.gameObject.tag}");
        
        if (other.CompareTag("BulletEnemy"))
        {
            PlayerHealthSystem hp = GetComponentInParent<PlayerHealthSystem>();
            if (hp != null)
                hp.TakeDamage(1);
        }
    }
}