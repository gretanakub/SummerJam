using UnityEngine;
using System;

public class DryingRackCounter : MonoBehaviour, IKitchenObjectParent, ICounter
{
    public event Action<float> OnDryingProgressChanged;
    public event Action OnDryingDone;

    [SerializeField] private DryingRecipeSO[] dryingRecipeSOArray;
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;
    private float dryingTimer;
    private bool isDrying;
    private DryingRecipeSO currentRecipe;

    private void Update()
    {
        if (isDrying && HasKitchenObject())
        {
            dryingTimer += Time.deltaTime;

            float progressNormalized = dryingTimer / currentRecipe.dryingTimeMax;
            OnDryingProgressChanged?.Invoke(progressNormalized);

            if (dryingTimer >= currentRecipe.dryingTimeMax)
            {
                // ตากเสร็จ
                isDrying = false;
                dryingTimer = 0f;

                KitchenObjectSO outputSO = currentRecipe.output;
                kitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(outputSO, this);

                OnDryingProgressChanged?.Invoke(0f);
                OnDryingDone?.Invoke();
            }
        }
    }

    public void Interact(PlayerController player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                DryingRecipeSO recipe = GetRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO());

                if (recipe != null)
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    currentRecipe = recipe;
                    dryingTimer = 0f;
                    isDrying = true; // เริ่มตากอัตโนมัติเลย
                    OnDryingProgressChanged?.Invoke(0f);
                }
                else
                {
                    Debug.Log("ของนี้ตากไม่ได้");
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                // หยิบได้เสมอ ไม่ว่าจะตากเสร็จหรือยัง
                isDrying = false;
                dryingTimer = 0f;
                kitchenObject.SetKitchenObjectParent(player);
                OnDryingProgressChanged?.Invoke(0f);
            }
            else
            {
                Debug.Log("player ถือของอยู่แล้ว");
            }
        }
    }

    // DryingRack ไม่มี InteractAlternate เพราะตากอัตโนมัติ
    public void InteractAlternate(PlayerController player) { }

    private DryingRecipeSO GetRecipeWithInput(KitchenObjectSO inputSO)
    {
        foreach (DryingRecipeSO recipe in dryingRecipeSOArray)
        {
            if (recipe.input == inputSO)
                return recipe;
        }
        return null;
    }

    public bool IsDrying() => isDrying;

    public Transform GetKitchenObjectFollowTransform() => counterTopPoint;
    public void SetKitchenObject(KitchenObject obj) => kitchenObject = obj;
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void ClearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;
}