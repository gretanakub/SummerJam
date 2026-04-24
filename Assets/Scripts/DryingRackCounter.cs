using UnityEngine;
using System;
using System.Collections.Generic;

public class DryingRackCounter : MonoBehaviour, IKitchenObjectParent, ICounter
{
    public event Action<float> OnDryingProgressChanged;
    public event Action OnDryingDone;
    public event Action OnIngredientAdded;

    [SerializeField] private DryingRecipeSO[] dryingRecipeSOArray;
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;
    private List<KitchenObjectSO> ingredientList = new List<KitchenObjectSO>();
    private float dryingTimer;
    private bool isDrying;
    private DryingRecipeSO currentRecipe;

    private void Update()
    {
        if (isDrying)
        {
            dryingTimer += Time.deltaTime;

            float progressNormalized = dryingTimer / currentRecipe.dryingTimeMax;
            OnDryingProgressChanged?.Invoke(progressNormalized);

            if (dryingTimer >= currentRecipe.dryingTimeMax)
            {
                isDrying = false;
                dryingTimer = 0f;

                // ลบของเก่าทั้งหมด
                ingredientList.Clear();
                if (kitchenObject != null)
                    kitchenObject.DestroySelf();

                KitchenObject.SpawnKitchenObject(currentRecipe.output, this);

                OnDryingProgressChanged?.Invoke(0f);
                OnDryingDone?.Invoke();
            }
        }
    }

    public void Interact(PlayerController player)
    {
        if (!isDrying)
        {
            if (player.HasKitchenObject())
            {
                KitchenObjectSO ingredient = player.GetKitchenObject().GetKitchenObjectSO();

                if (IsValidIngredient(ingredient))
                {
                    ingredientList.Add(ingredient);
                    player.GetKitchenObject().DestroySelf();
                    OnIngredientAdded?.Invoke();

                    // เช็คว่า ingredient ครบตาม recipe ไหม → เริ่มตากอัตโนมัติ
                    DryingRecipeSO recipe = GetMatchingRecipe();
                    if (recipe != null)
                    {
                        currentRecipe = recipe;
                        dryingTimer = 0f;
                        isDrying = true;
                        Debug.Log("เริ่มตาก!");
                    }
                    else
                    {
                        Debug.Log("ใส่ " + ingredient.objectName + " แล้ว รอใส่เพิ่ม");
                    }
                }
                else
                {
                    Debug.Log("ของนี้ตากไม่ได้");
                }
            }
            else
            {
                // หยิบของออก
                if (HasKitchenObject())
                {
                    kitchenObject.SetKitchenObjectParent(player);
                    ingredientList.Clear();
                    OnDryingProgressChanged?.Invoke(0f);
                    OnIngredientAdded?.Invoke();
                }
            }
        }
        else
        {
            // กำลังตากอยู่ → หยิบออกได้ แต่ reset ทุกอย่าง
            if (!player.HasKitchenObject())
            {
                isDrying = false;
                dryingTimer = 0f;
                ingredientList.Clear();

                if (HasKitchenObject())
                    kitchenObject.SetKitchenObjectParent(player);

                OnDryingProgressChanged?.Invoke(0f);
                OnIngredientAdded?.Invoke();
            }
        }
    }

    public void InteractAlternate(PlayerController player) { }

    private bool IsValidIngredient(KitchenObjectSO ingredientSO)
    {
        foreach (DryingRecipeSO recipe in dryingRecipeSOArray)
        {
            foreach (KitchenObjectSO input in recipe.inputArray)
            {
                if (input == ingredientSO)
                    return true;
            }
        }
        return false;
    }

    private DryingRecipeSO GetMatchingRecipe()
    {
        foreach (DryingRecipeSO recipe in dryingRecipeSOArray)
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
    public bool IsDrying() => isDrying;

    public Transform GetKitchenObjectFollowTransform() => counterTopPoint;
    public void SetKitchenObject(KitchenObject obj) => kitchenObject = obj;
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void ClearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;
}