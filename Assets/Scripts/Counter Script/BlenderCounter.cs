using UnityEngine;
using System;
using System.Collections.Generic;

public class BlenderCounter : MonoBehaviour, IKitchenObjectParent, ICounter
{
    public event Action<float> OnBlendingProgressChanged;
    public event Action OnBlendingStarted;
    public event Action OnBlendingDone;
    public event Action OnIngredientAdded;

    [SerializeField] private BlenderRecipeSO[] blenderRecipeSOArray;
    [SerializeField] private KitchenObjectSO cupOfIceSO;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private BlenderSlot[] ingredientSlots; // slots สำหรับวางของ
    [SerializeField] private float blendingTimeMax = 3f;

    private KitchenObject kitchenObject;
    private List<KitchenObjectSO> ingredientList = new List<KitchenObjectSO>();
    private float blendingTimer;
    private bool isBlending;
    private bool blendingDone;
    private BlenderRecipeSO completedRecipe;

    private void Update()
    {
        if (isBlending)
        {
            blendingTimer += Time.deltaTime;

            float progressNormalized = blendingTimer / blendingTimeMax;
            OnBlendingProgressChanged?.Invoke(progressNormalized);

            if (blendingTimer >= blendingTimeMax)
            {
                isBlending = false;
                blendingTimer = 0f;
                blendingDone = true;

                // ซ่อนของทั้งหมดใน slot
                ClearAllSlots();

                OnBlendingProgressChanged?.Invoke(1f);
                OnBlendingDone?.Invoke();
                Debug.Log("ปั่นเสร็จแล้ว! นำ Cup of Ice มากดเพื่อรับเมนู");
            }
        }
    }

    public void Interact(PlayerController player)
    {
        if (!blendingDone)
        {
            if (player.HasKitchenObject())
            {
                KitchenObjectSO ingredient = player.GetKitchenObject().GetKitchenObjectSO();

                if (IsValidIngredient(ingredient) && !isBlending)
                {
                    // หา slot ว่าง
                    BlenderSlot freeSlot = GetFreeSlot();

                    if (freeSlot != null)
                    {
                        // วางของลง slot จริงๆ แทน DestroySelf
                        player.GetKitchenObject().SetKitchenObjectParent(freeSlot);
                        ingredientList.Add(ingredient);
                        OnIngredientAdded?.Invoke();
                        Debug.Log("ใส่ " + ingredient.objectName + " เข้า Blender แล้ว (" + ingredientList.Count + " ตัว)");
                    }
                    else
                    {
                        Debug.Log("Blender เต็มแล้ว");
                    }
                }
                else
                {
                    Debug.Log("ของนี้ใส่ใน Blender ไม่ได้หรือกำลังปั่นอยู่");
                }
            }
            else
            {
                // ไม่ถือของ → เอา ingredient ออกทีละตัว
                if (!isBlending && ingredientList.Count > 0)
                {
                    KitchenObjectSO lastIngredient = ingredientList[ingredientList.Count - 1];
                    ingredientList.RemoveAt(ingredientList.Count - 1);

                    // หา slot ที่มีของตัวนั้น
                    BlenderSlot slot = GetSlotWithIngredient(lastIngredient);
                    if (slot != null)
                        slot.GetKitchenObject().SetKitchenObjectParent(player);

                    OnIngredientAdded?.Invoke();
                    Debug.Log("เอา " + lastIngredient.objectName + " ออกจาก Blender แล้ว");
                }
                else if (isBlending)
                {
                    Debug.Log("กำลังปั่นอยู่ ไม่สามารถเอาของออกได้");
                }
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().GetKitchenObjectSO() == cupOfIceSO)
                {
                    player.GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(completedRecipe.output, player);

                    ingredientList.Clear();
                    blendingDone = false;
                    completedRecipe = null;

                    OnBlendingProgressChanged?.Invoke(0f);
                    OnIngredientAdded?.Invoke();
                    Debug.Log("ได้รับเมนูสำเร็จ!");
                }
                else
                {
                    Debug.Log("ต้องถือ Cup of Ice ถึงจะรับเมนูได้!");
                }
            }
            else
            {
                Debug.Log("ต้องถือ Cup of Ice ก่อน!");
            }
        }
    }

    public void InteractAlternate(PlayerController player)
    {
        if (!isBlending && !blendingDone && ingredientList.Count > 0)
        {
            BlenderRecipeSO recipe = GetMatchingRecipe();

            if (recipe != null)
            {
                isBlending = true;
                blendingTimer = 0f;
                completedRecipe = recipe;
                OnBlendingStarted?.Invoke();
                Debug.Log("เริ่มปั่น!");
            }
            else
            {
                Debug.Log("ingredient ยังไม่ครบตาม recipe");
            }
        }
    }

    private BlenderSlot GetFreeSlot()
    {
        foreach (BlenderSlot slot in ingredientSlots)
        {
            if (!slot.HasKitchenObject())
                return slot;
        }
        return null;
    }

    private BlenderSlot GetSlotWithIngredient(KitchenObjectSO so)
    {
        foreach (BlenderSlot slot in ingredientSlots)
        {
            if (slot.HasKitchenObject() &&
                slot.GetKitchenObject().GetKitchenObjectSO() == so)
                return slot;
        }
        return null;
    }

    private void ClearAllSlots()
    {
        foreach (BlenderSlot slot in ingredientSlots)
        {
            if (slot.HasKitchenObject())
                slot.GetKitchenObject().DestroySelf();
        }
    }

    private bool IsValidIngredient(KitchenObjectSO ingredientSO)
    {
        foreach (BlenderRecipeSO recipe in blenderRecipeSOArray)
        {
            foreach (KitchenObjectSO input in recipe.inputArray)
            {
                if (input == ingredientSO)
                    return true;
            }
        }
        return false;
    }

    private BlenderRecipeSO GetMatchingRecipe()
    {
        foreach (BlenderRecipeSO recipe in blenderRecipeSOArray)
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
    public bool IsBlending() => isBlending;
    public bool IsBlendingDone() => blendingDone;

    public Transform GetKitchenObjectFollowTransform() => counterTopPoint;
    public void SetKitchenObject(KitchenObject obj) => kitchenObject = obj;
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void ClearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;
}