using UnityEngine;
using System;

public class CuttingCounter : MonoBehaviour, IKitchenObjectParent, ICounter
{
    public event Action<float> OnCuttingProgressChanged; // ส่งค่า 0-1 ให้ UI

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;
    private int cuttingProgress;
    private int cuttingProgressMax;

    public void Interact(PlayerController player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    // หา max จาก recipe
                    CuttingRecipeSO recipe = GetRecipeWithInput(kitchenObject.GetKitchenObjectSO());
                    cuttingProgressMax = recipe.cuttingProgressMax;

                    // อัพเดท UI เป็น 0
                    OnCuttingProgressChanged?.Invoke(0f);
                }
                else
                {
                    Debug.Log("ของนี้หั่นไม่ได้");
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                kitchenObject.SetKitchenObjectParent(player);

                // Reset UI เป็น 0 ตอนหยิบออก
                OnCuttingProgressChanged?.Invoke(0f);
            }
            else
            {
                Debug.Log("player ถือของอยู่แล้ว");
            }
        }
    }

    public void InteractAlternate(PlayerController player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(kitchenObject.GetKitchenObjectSO()))
        {
            cuttingProgress++;

            // อัพเดท progress bar (0-1)
            float progressNormalized = (float)cuttingProgress / cuttingProgressMax;
            OnCuttingProgressChanged?.Invoke(progressNormalized);

            CuttingRecipeSO recipe = GetRecipeWithInput(kitchenObject.GetKitchenObjectSO());

            if (cuttingProgress >= recipe.cuttingProgressMax)
            {
                KitchenObjectSO outputSO = recipe.output;
                kitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(outputSO, this);
                cuttingProgress = 0;

                // Reset UI หลังหั่นเสร็จ
                OnCuttingProgressChanged?.Invoke(0f);
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