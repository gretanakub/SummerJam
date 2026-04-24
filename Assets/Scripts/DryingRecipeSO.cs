using UnityEngine;

[CreateAssetMenu(fileName = "DryingRecipeSO", menuName = "ScriptableObjects/DryingRecipeSO")]
public class DryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO[] inputArray; // รับ input หลายตัว
    public KitchenObjectSO output;
    public float dryingTimeMax;
}