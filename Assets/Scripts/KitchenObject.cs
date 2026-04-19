using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent parent)
    {
        if (kitchenObjectParent != null)
            kitchenObjectParent.ClearKitchenObject();

        kitchenObjectParent = parent;

        if (parent.HasKitchenObject())
            Debug.LogError("Parent already has a KitchenObject!");

        parent.SetKitchenObject(this);

        // ตอนถือ → Kinematic ไม่ให้ physics รบกวน
        if (rb != null) rb.isKinematic = true;

        transform.parent = parent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop(Vector3 dropPosition)
    {
        kitchenObjectParent.ClearKitchenObject();
        kitchenObjectParent = null;

        transform.parent = null;
        transform.position = dropPosition;

        // ตอน drop → เปิด physics ให้ตกพื้น
        if (rb != null) rb.isKinematic = false;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent parent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(parent);
        return kitchenObject;
    }
}