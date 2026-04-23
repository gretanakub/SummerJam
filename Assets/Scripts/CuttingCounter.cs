using UnityEngine;

public class CuttingCounter : MonoBehaviour, IKitchenObjectParent, ICounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray; // recipe ทั้งหมด
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;
    private int cuttingProgress;

    public void Interact(PlayerController player)
    {
        if (!HasKitchenObject())
        {
            // Counter ว่าง
            if (player.HasKitchenObject())
            {
                // เช็คว่าของที่ถืออยู่หั่นได้ไหม
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // วางของลง counter แล้ว reset progress
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                }
                else
                {
                    Debug.Log("ของนี้หั่นไม่ได้");
                }
            }
        }
        else
        {
            // Counter มีของอยู่
            if (!player.HasKitchenObject())
            {
                // หยิบของจาก counter
                kitchenObject.SetKitchenObjectParent(player);
            }
            else
            {
                Debug.Log("player ถือของอยู่แล้ว");
            }
        }
    }

    public void InteractAlternate(PlayerController player)
    {
        // กด alternate (F / Button West) เพื่อหั่น
        if (HasKitchenObject() && HasRecipeWithInput(kitchenObject.GetKitchenObjectSO()))
        {
            cuttingProgress++;

            CuttingRecipeSO recipe = GetRecipeWithInput(kitchenObject.GetKitchenObjectSO());

            if (cuttingProgress >= recipe.cuttingProgressMax)
            {
                // หั่นครบแล้ว → เปลี่ยนเป็น output
                KitchenObjectSO outputSO = recipe.output;
                kitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(outputSO, this);
                cuttingProgress = 0;
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputSO)
    {
        return GetRecipeWithInput(inputSO) != null;
    }

    private CuttingRecipeSO GetRecipeWithInput(KitchenObjectSO inputSO)
    {
        foreach (CuttingRecipeSO recipe in cuttingRecipeSOArray)
        {
            if (recipe.input == inputSO)
                return recipe;
        }
        return null;
    }

    public Transform GetKitchenObjectFollowTransform() => counterTopPoint;
    public void SetKitchenObject(KitchenObject obj) => kitchenObject = obj;
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void ClearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;
}