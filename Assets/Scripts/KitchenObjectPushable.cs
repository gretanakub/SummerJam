using UnityEngine;

public class KitchenObjectPushable : MonoBehaviour
{
    [SerializeField] private float pushForce = 3f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        // เช็คว่าชนกับ player
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            // ผลักของออกจาก player
            Vector3 pushDir = transform.position - collision.transform.position;
            pushDir.y = 0;
            pushDir.Normalize();

            if (rb != null && !rb.isKinematic)
            {
                rb.AddForce(pushDir * pushForce, ForceMode.Force);
            }
        }
    }
}