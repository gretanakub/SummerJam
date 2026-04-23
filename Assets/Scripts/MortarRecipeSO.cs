using UnityEngine;

[CreateAssetMenu(fileName = "MortarRecipeSO", menuName = "ScriptableObjects/MortarRecipeSO")]
public class MortarRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public int grindingProgressMax;
}