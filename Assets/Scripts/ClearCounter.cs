using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint; // จุดวาง object บน counter

    private KitchenObject kitchenObject;

    public void Interact(PlayerController player)
    {
        if (!HasKitchenObject())
        {
            // Counter ว่าง → ถ้า player ถือของอยู่ให้วางลง
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // ทั้งคู่ว่าง ไม่ทำอะไร
            }
        }
        else
        {
            // Counter มีของอยู่
            if (player.HasKitchenObject())
            {
                // player ถือของอยู่ด้วย ไม่ทำอะไร
            }
            else
            {
                // player ไม่ถือของ → หยิบของจาก counter
                kitchenObject.SetKitchenObjectParent(player);
            }
        }
    }

    // IKitchenObjectParent
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject obj)
    {
        kitchenObject = obj;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}