using UnityEngine;
using System;

public class MortarCounter : MonoBehaviour, IKitchenObjectParent, ICounter
{
    public event Action<float> OnGrindingProgressChanged;

    [SerializeField] private MortarRecipeSO[] mortarRecipeSOArray;
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;
    private int grindingProgress;
    private int grindingProgressMax;

    public void Interact(PlayerController player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    grindingProgress = 0;

                    MortarRecipeSO recipe = GetRecipeWithInput(kitchenObject.GetKitchenObjectSO());
                    grindingProgressMax = recipe.grindingProgressMax;

                    OnGrindingProgressChanged?.Invoke(0f);
                }
                else
                {
                    Debug.Log("ของนี้บดไม่ได้");
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                kitchenObject.SetKitchenObjectParent(player);
                OnGrindingProgressChanged?.Invoke(0f);
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
            grindingProgress++;

            float progressNormalized = (float)grindingProgress / grindingProgressMax;
            OnGrindingProgressChanged?.Invoke(progressNormalized);

            MortarRecipeSO recipe = GetRecipeWithInput(kitchenObject.GetKitchenObjectSO());

            if (grindingProgress >= recipe.grindingProgressMax)
            {
                KitchenObjectSO outputSO = recipe.output;
                kitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(outputSO, this);
                grindingProgress = 0;

                OnGrindingProgressChanged?.Invoke(0f);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputSO)
    {
        return GetRecipeWithInput(inputSO) != null;
    }

    private MortarRecipeSO GetRecipeWithInput(KitchenObjectSO inputSO)
    {
        foreach (MortarRecipeSO recipe in mortarRecipeSOArray)
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