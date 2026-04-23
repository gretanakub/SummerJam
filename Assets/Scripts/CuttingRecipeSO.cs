using UnityEngine;

[CreateAssetMenu(fileName = "CuttingRecipeSO", menuName = "ScriptableObjects/CuttingRecipeSO")]
public class CuttingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;   // ของที่จะหั่น เช่น Orange
    public KitchenObjectSO output;  // ของที่ได้หลังหั่น เช่น OrangeSlice
    public int cuttingProgressMax;  // จำนวนครั้งที่ต้องหั่น
}