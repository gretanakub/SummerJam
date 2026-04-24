using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 1;
    public float pickupRadius = 1f;
    private bool collected = false;

    void Update()
    {
        if (collected) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealthSystem hp = hit.GetComponent<PlayerHealthSystem>();
                if (hp != null)
                {
                    collected = true;
                    hp.Heal(healAmount);
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }
}