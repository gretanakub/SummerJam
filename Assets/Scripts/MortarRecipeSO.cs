using UnityEngine;

[CreateAssetMenu(fileName = "MortarRecipeSO", menuName = "ScriptableObjects/MortarRecipeSO")]
public class MortarRecipeSO : ScriptableObject
{
    public KitchenObjectSO[] inputArray; // รับ input หลายตัว
    public KitchenObjectSO output;
    public int grindingProgressMax;
}