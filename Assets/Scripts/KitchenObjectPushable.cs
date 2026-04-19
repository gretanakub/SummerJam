using UnityEngine;

public class KitchenObjectPushable : MonoBehaviour
{
    [SerializeField] private float pushForce = 3f;
    [SerializeField] private float torqueForce = 2f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            Vector3 pushDir = transform.position - collision.transform.position;
            pushDir.y = 0;
            pushDir.Normalize();

            if (rb != null && !rb.isKinematic)
            {
                // ผลักไปข้างหน้า
                rb.AddForce(pushDir * pushForce, ForceMode.Impulse);

                // หมุนให้กลิ้ง
                Vector3 torqueDir = Vector3.Cross(Vector3.up, pushDir);
                rb.AddTorque(torqueDir * torqueForce, ForceMode.Impulse);
            }
        }
    }
}