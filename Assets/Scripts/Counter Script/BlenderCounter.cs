using UnityEngine;
using System;
using System.Collections.Generic;

public class BlenderCounter : MonoBehaviour, IKitchenObjectParent, ICounter
{
    public event Action<float> OnBlendingProgressChanged;
    public event Action OnBlendingStarted;
    public event Action OnBlendingDone;

    [SerializeField] private BlenderRecipeSO[] blenderRecipeSOArray;
    [SerializeField] private KitchenObjectSO cupOfIceSO;  // Cup of Ice SO
    [SerializeField] private KitchenObjectSO readyToServeSO; // น้ำพร้อมเสิร์ฟ
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private float blendingTimeMax = 3f;

    private KitchenObject kitchenObject; // Cup of Ice ที่วางอยู่
    private List<KitchenObjectSO> ingredientList = new List<KitchenObjectSO>();
    private float blendingTimer;
    private bool isBlending;
    private bool hasCupOfIce;

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

                // เช็ค recipe ว่า ingredient ที่ใส่ตรงกับ recipe ไหน
                BlenderRecipeSO recipe = GetMatchingRecipe();

                if (recipe != null)
                {
                    // ลบ ingredient ทั้งหมด
                    ingredientList.Clear();

                    // ทำลาย Cup of Ice เดิม
                    if (kitchenObject != null)
                        kitchenObject.DestroySelf();

                    // Spawn น้ำพร้อมเสิร์ฟ
                    KitchenObject.SpawnKitchenObject(recipe.output, this);
                }
                else
                {
                    Debug.Log("ไม่มี recipe ที่ตรงกับ ingredient ที่ใส่");
                    isBlending = false;
                }

                OnBlendingProgressChanged?.Invoke(0f);
                OnBlendingDone?.Invoke();
            }
        }
    }

    public void Interact(PlayerController player)
    {
        if (!hasCupOfIce)
        {
            // ยังไม่มี Cup of Ice → ต้องวาง Cup of Ice ก่อน
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().GetKitchenObjectSO() == cupOfIceSO)
                {
                    // วาง Cup of Ice ลง Blender
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    hasCupOfIce = true;
                    ingredientList.Clear();
                    Debug.Log("วาง Cup of Ice ลง Blender แล้ว");
                }
                else
                {
                    Debug.Log("ต้องใส่ Cup of Ice ก่อน!");
                }
            }
        }
        else
        {
            // มี Cup of Ice แล้ว
            if (player.HasKitchenObject())
            {
                KitchenObjectSO ingredient = player.GetKitchenObject().GetKitchenObjectSO();

                // เช็คว่า ingredient นี้อยู่ใน recipe ไหนสักอัน
                if (IsValidIngredient(ingredient) && !isBlending)
                {
                    // เพิ่ม ingredient เข้า list
                    ingredientList.Add(ingredient);

                    // ทำลาย ingredient ที่ถืออยู่
                    player.GetKitchenObject().DestroySelf();

                    Debug.Log("ใส่ " + ingredient.objectName + " เข้า Blender แล้ว");
                    Debug.Log("Ingredient ที่ใส่แล้ว: " + ingredientList.Count + " ตัว");
                }
                else
                {
                    Debug.Log("ของนี้ใส่ใน Blender ไม่ได้");
                }
            }
            else
            {
                // player ไม่ถือของ
                if (!isBlending && HasKitchenObject())
                {
                    // หยิบน้ำพร้อมเสิร์ฟออกมา
                    kitchenObject.SetKitchenObjectParent(player);
                    hasCupOfIce = false;
                    ingredientList.Clear();
                    OnBlendingProgressChanged?.Invoke(0f);
                }
            }
        }
    }

    public void InteractAlternate(PlayerController player)
    {
        // กด F เพื่อเริ่มปั่น ต้องมี Cup of Ice และ ingredient อย่างน้อย 1 ตัว
        if (hasCupOfIce && ingredientList.Count > 0 && !isBlending)
        {
            BlenderRecipeSO recipe = GetMatchingRecipe();

            if (recipe != null)
            {
                isBlending = true;
                blendingTimer = 0f;
                OnBlendingStarted?.Invoke();
                Debug.Log("เริ่มปั่น!");
            }
            else
            {
                Debug.Log("ingredient ยังไม่ครบตาม recipe");
            }
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

            // เช็คว่า ingredient ทุกตัวใน recipe ตรงกับที่ใส่ไปไหม
            List<KitchenObjectSO> tempList = new List<KitchenObjectSO>(ingredientList);

            foreach (KitchenObjectSO input in recipe.inputArray)
            {
                if (tempList.Contains(input))
                {
                    tempList.Remove(input);
                }
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
    public bool HasCupOfIce() => hasCupOfIce;
    public bool IsBlending() => isBlending;

    public Transform GetKitchenObjectFollowTransform() => counterTopPoint;
    public void SetKitchenObject(KitchenObject obj) => kitchenObject = obj;
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void ClearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;
}