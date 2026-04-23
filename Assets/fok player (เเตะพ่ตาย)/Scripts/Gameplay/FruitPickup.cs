using UnityEngine;

public class FruitPickup : MonoBehaviour
{
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
                    weapon.SetWeaponLocked(true); // ล็อคปืน
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }
}