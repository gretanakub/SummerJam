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

    public KitchenObjectSO GetKitchenObjectSO() => kitchenObjectSO;

    public void SetKitchenObjectParent(IKitchenObjectParent parent)
    {
        if (kitchenObjectParent != null)
            kitchenObjectParent.ClearKitchenObject();

        kitchenObjectParent = parent;

        if (parent.HasKitchenObject())
            Debug.LogError("Parent already has a KitchenObject!");

        parent.SetKitchenObject(this);

        if (rb != null) rb.isKinematic = true;

        if (TryGetComponent(out KitchenObjectCollision col))
            col.SetColliderActive(false);

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

        if (rb != null) rb.isKinematic = false;

        if (TryGetComponent(out KitchenObjectCollision col))
            col.SetColliderActive(true);
    }

    public IKitchenObjectParent GetKitchenObjectParent() => kitchenObjectParent;

    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent parent)
    {
        Debug.Log("SpawnKitchenObject called");
        Debug.Log("kitchenObjectSO = " + kitchenObjectSO);
        Debug.Log("kitchenObjectSO.prefab = " + kitchenObjectSO?.prefab);
        Debug.Log("parent = " + parent);

        if (kitchenObjectSO == null)
        {
            Debug.LogError("kitchenObjectSO is NULL!");
            return null;
        }

        if (kitchenObjectSO.prefab == null)
        {
            Debug.LogError("prefab in kitchenObjectSO is NULL!");
            return null;
        }

        if (parent == null)
        {
            Debug.LogError("parent is NULL!");
            return null;
        }

        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(parent);
        return kitchenObject;
    }
}