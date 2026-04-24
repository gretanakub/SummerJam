using UnityEngine;

public class BlenderSlot : MonoBehaviour, IKitchenObjectParent
{
    private KitchenObject kitchenObject;

    public Transform GetKitchenObjectFollowTransform() => transform;
    public void SetKitchenObject(KitchenObject obj) => kitchenObject = obj;
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void ClearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;
}