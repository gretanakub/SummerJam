using UnityEngine;
using System;
using System.Collections.Generic;

public class MortarCounter : MonoBehaviour, IKitchenObjectParent, ICounter
{
    public event Action<float> OnGrindingProgressChanged;
    public event Action OnIngredientAdded;

    [SerializeField] private MortarRecipeSO[] mortarRecipeSOArray;
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;
    private List<KitchenObjectSO> ingredientList = new List<KitchenObjectSO>();
    private int grindingProgress;
    private int grindingProgressMax;
    private MortarRecipeSO currentRecipe;

    public void Interact(PlayerController player)
    {
        if (player.HasKitchenObject())
        {
            KitchenObjectSO ingredient = player.GetKitchenObject().GetKitchenObjectSO();

            if (IsValidIngredient(ingredient) && !IsReadyToGrind())
            {
                ingredientList.Add(ingredient);
                player.GetKitchenObject().DestroySelf();
                OnIngredientAdded?.Invoke();

                // เช็คว่าครบ recipe ไหม
                MortarRecipeSO recipe = GetMatchingRecipe();
                if (recipe != null)
                {
                    currentRecipe = recipe;
                    grindingProgress = 0;
                    grindingProgressMax = recipe.grindingProgressMax;
                    OnGrindingProgressChanged?.Invoke(0f);
                    Debug.Log("ingredient ครบแล้ว กด F เพื่อบด");
                }
                else
                {
                    Debug.Log("ใส่ " + ingredient.objectName + " แล้ว รอใส่เพิ่ม");
                }
            }
            else
            {
                Debug.Log("ของนี้บดไม่ได้หรือยังใส่ ingredient ไม่ครบ");
            }
        }
        else
        {
            // หยิบของออก
            if (HasKitchenObject())
            {
                kitchenObject.SetKitchenObjectParent(player);
                ingredientList.Clear();
                grindingProgress = 0;
                currentRecipe = null;
                OnGrindingProgressChanged?.Invoke(0f);
                OnIngredientAdded?.Invoke();
            }
            else if (ingredientList.Count > 0)
            {
                // ยังไม่มีของบน counter แต่มี ingredient → reset
                ingredientList.Clear();
                grindingProgress = 0;
                currentRecipe = null;
                OnGrindingProgressChanged?.Invoke(0f);
                OnIngredientAdded?.Invoke();
            }
        }
    }

    public void InteractAlternate(PlayerController player)
    {
        if (IsReadyToGrind())
        {
            grindingProgress++;

            float progressNormalized = (float)grindingProgress / grindingProgressMax;
            OnGrindingProgressChanged?.Invoke(progressNormalized);

            if (grindingProgress >= grindingProgressMax)
            {
                KitchenObjectSO outputSO = currentRecipe.output;

                if (HasKitchenObject())
                    kitchenObject.DestroySelf();

                KitchenObject.SpawnKitchenObject(outputSO, this);

                ingredientList.Clear();
                grindingProgress = 0;
                currentRecipe = null;

                OnGrindingProgressChanged?.Invoke(0f);
                OnIngredientAdded?.Invoke();
            }
        }
    }

    private bool IsReadyToGrind() => currentRecipe != null;

    private bool IsValidIngredient(KitchenObjectSO ingredientSO)
    {
        foreach (MortarRecipeSO recipe in mortarRecipeSOArray)
        {
            foreach (KitchenObjectSO input in recipe.inputArray)
            {
                if (input == ingredientSO)
                    return true;
            }
        }
        return false;
    }

    private MortarRecipeSO GetMatchingRecipe()
    {
        foreach (MortarRecipeSO recipe in mortarRecipeSOArray)
        {
            if (recipe.inputArray.Length != ingredientList.Count)
                continue;

            bool isMatch = true;
            List<KitchenObjectSO> tempList = new List<KitchenObjectSO>(ingredientList);

            foreach (KitchenObjectSO input in recipe.inputArray)
            {
                if (tempList.Contains(input))
                    tempList.Remove(input);
                else
                {
                    isMatch = false;
                    break;
                }
            }

            if (isMatch) return recipe;
        }
        return null;
    }

    public List<KitchenObjectSO> GetIngredientList() => ingredientList;

    public Transform GetKitchenObjectFollowTransform() => counterTopPoint;
    public void SetKitchenObject(KitchenObject obj) => kitchenObject = obj;
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void ClearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;
}