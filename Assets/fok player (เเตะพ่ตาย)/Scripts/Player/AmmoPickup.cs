using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 10;
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
                WeaponSystem weapon = hit.GetComponent<WeaponSystem>();
                if (weapon != null)
                {
                    collected = true;
                    weapon.AddAmmo(ammoAmount);
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }
}