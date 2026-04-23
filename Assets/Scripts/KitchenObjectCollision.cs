using UnityEngine;

public class KitchenObjectCollision : MonoBehaviour
{
    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    public void SetColliderActive(bool active)
    {
        if (col != null) col.enabled = active;
    }
}