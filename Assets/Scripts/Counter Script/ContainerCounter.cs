using UnityEngine;

public class ContainerCounter : MonoBehaviour, IKitchenObjectParent, ICounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public void Interact(PlayerController player)
    {
        if (!player.HasKitchenObject())
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
    }

    public Transform GetKitchenObjectFollowTransform() => counterTopPoint;
    public void SetKitchenObject(KitchenObject obj) => kitchenObject = obj;
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void ClearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;
}