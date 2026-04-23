using UnityEngine;
using System;

public class BlenderCounter : MonoBehaviour, IKitchenObjectParent, ICounter
{
    public event Action<float> OnBlendingProgressChanged;
    public event Action OnBlendingStarted;
    public event Action OnBlendingDone;

    [SerializeField] private BlenderRecipeSO[] blenderRecipeSOArray;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private float blendingTimeMax = 3f; // วินาทีที่ใช้ปั่น

    private KitchenObject kitchenObject;
    private float blendingTimer;
    private bool isBlending;

    private void Update()
    {
        if (isBlending && HasKitchenObject())
        {
            blendingTimer += Time.deltaTime;

            // อัพเดท progress
            float progressNormalized = blendingTimer / blendingTimeMax;
            OnBlendingProgressChanged?.Invoke(progressNormalized);

            if (blendingTimer >= blendingTimeMax)
            {
                // ปั่นเสร็จ
                isBlending = false;
                blendingTimer = 0f;

                BlenderRecipeSO recipe = GetRecipeWithInput(kitchenObject.GetKitchenObjectSO());

                if (recipe != null)
                {
                    KitchenObjectSO outputSO = recipe.output;
                    kitchenObject.DestroySelf();
                    KitchenObject.SpawnKitchenObject(outputSO, this);
                }

                OnBlendingProgressChanged?.Invoke(0f);
                OnBlendingDone?.Invoke();
            }
        }
    }

    public void Interact(PlayerController player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                // เช็คว่าของที่ถืออยู่ปั่นได้ไหม
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    blendingTimer = 0f;
                    isBlending = false;
                    OnBlendingProgressChanged?.Invoke(0f);
                }
                else
                {
                    Debug.Log("ของนี้ปั่นไม่ได้");
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                // ถ้าปั่นเสร็จแล้ว → หยิบได้
                if (!isBlending)
                {
                    kitchenObject.SetKitchenObjectParent(player);
                    OnBlendingProgressChanged?.Invoke(0f);
                }
                else
                {
                    Debug.Log("กำลังปั่นอยู่ รอให้เสร็จก่อน");
                }
            }
        }
    }

    public void InteractAlternate(PlayerController player)
    {
        // กด F เพื่อเริ่ม/หยุดปั่น
        if (HasKitchenObject() && HasRecipeWithInput(kitchenObject.GetKitchenObjectSO()))
        {
            isBlending = !isBlending;

            if (isBlending)
            {
                OnBlendingStarted?.Invoke();
            }
            else
            {
                OnBlendingProgressChanged?.Invoke(0f);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputSO)
    {
        return GetRecipeWithInput(inputSO) != null;
    }

    private BlenderRecipeSO GetRecipeWithInput(KitchenObjectSO inputSO)
    {
        foreach (BlenderRecipeSO recipe in blenderRecipeSOArray)
        {
            if (recipe.input == inputSO)
                return recipe;
        }
        return null;
    }

    public bool IsBlending() => isBlending;

    public Transform GetKitchenObjectFollowTransform() => counterTopPoint;
    public void SetKitchenObject(KitchenObject obj) => kitchenObject = obj;
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void ClearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;
}